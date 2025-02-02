using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ServerCore
{
    //미리 최대 버퍼크기를 잡아둬서 안정적으로 메모리 내에서 관리..
    // 쵸 간단 프로젝트 
    //1. 최대 동접을 정해서 그 바운더리 안에서  모든 것이 이루어지게 하기
    //2. 최대 동접은 없더라도 그냥 일단 _freeBuffers를 추가해서 할당하기....
    //=== 우선 1번 방향으로 설정 === 이루어지지 못한다면 그냥 DisConnect로 끝내기  

    internal class BufferManager
    {

        private static readonly Lazy<BufferManager> _instance = new Lazy<BufferManager>(() => new BufferManager(SystemDef.MAX_BUFFER_COUNT,SystemDef.MAX_BUFFER_SIZE));

        // BufferManager 싱글톤 인스턴스
        public static BufferManager Instance => _instance.Value;


        //ArrayPool -> ThreadSafe
        private readonly ArrayPool<byte> _bufferPool; // ArrayPool을 사용하여 메모리 재사용
        private readonly int _bufferSize;
        private readonly object _stackLock = new object();

        //stack 은 안전하지 않음 ConcurrentStack 이 있음
        private readonly Stack<byte[]> _freeBuffers;


        public BufferManager(int initialBufferCount, int bufferSize)
        {
            _bufferPool = ArrayPool<byte>.Shared;
            _bufferSize = bufferSize;
            _freeBuffers = new Stack<byte[]>(initialBufferCount);

            // 미리 버퍼를 생성하여 스택에 추가
            for (int i = 0; i < initialBufferCount; i++)
            {
                _freeBuffers.Push(_bufferPool.Rent(bufferSize));
            }
        }

        /// <summary>
        /// 버퍼를 할당하여 SocketAsyncEventArgs에 추가
        /// //BufferList 에 add를 하면 되지 않음 
        /// </summary>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            lock (_stackLock) 
            {
                if (_freeBuffers.Count > 0)
                {
                    byte[] buffer = _freeBuffers.Pop();
                    args.BufferList ??= new List<ArraySegment<byte>>(); // BufferList가 없으면 초기화
                    var list = args.BufferList;
                    list.Add(new ArraySegment<byte>(buffer, 0, _bufferSize));
                    args.BufferList = list;
                    return true;
                }
            }

            return false; // 사용 가능한 버퍼가 없음
        }


        /// <summary>
        /// args에 존재하는 BufferList에 데이터를 쓸 수 있는 공간 확인
        /// 만약 실패한다면 추가적으로 BufferList를 SetBuffer 해서 얻어와도 되긴 함 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="bytesToWrite"></param>
        /// <returns></returns>


        ///===args.BytesTransferred== 크기 가 정해지 는 법 
        //Socket.SendAsync()를 호출하여 데이터를 전송할 때
        //args.BytesTransferred는 전송이 완료된 바이트 수를 기록하고,
        //다음에 새로운 전송 작업을 시작할 때는 다시 0으로 초기화되며 그 작업에서의 전송 바이트 수로 업데이트됩니다.
        public bool CanWrite(SocketAsyncEventArgs args)
        {
            lock (_stackLock)
            {
                if (args.BufferList == null || args.BufferList.Count == 0)
                {
                    return false;
                }

                foreach (var segment in args.BufferList)
                {
                    // 남은 공간 계산: 전체 버퍼 크기 - 현재 Offset
                    int remainingSpace = segment.Array!.Length - segment.Offset;

                    // 남은 공간이 전송된 바이트 수보다 충분한지 확인
                    if (remainingSpace >= args.BytesTransferred)
                    {
                        return true; // 남은 공간이 충분하다면 true 반환
                    }
                }
            }

            return false; // 쓰기 공간이 부족함
        }


        /// <summary>
        /// 사용이 끝난 버퍼를 반환
        /// </summary>
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            lock (_stackLock)
            {
                if (args.BufferList == null)
                {
                    return;
                }

                foreach (var segment in args.BufferList)
                {
                    if (segment.Array == null)
                    {
                        continue;
                    }

                    _bufferPool.Return(segment.Array);
                    _freeBuffers.Push(segment.Array);
                }

                args.BufferList.Clear(); // BufferList 초기화
                args.BufferList = null;
            }
        }
    }
}

namespace ServerCore
{
    public class SessionPool<T> where T : Session, new()
    {
        private readonly Stack<T> _sessionPool = new Stack<T>();
        private readonly object _lock = new object();

        // Singleton pattern for accessing the pool
        private static readonly Lazy<SessionPool<T>> _instance = new Lazy<SessionPool<T>>(() => new SessionPool<T>());

        public static SessionPool<T> Instance => _instance.Value;

        // 세션을 풀에서 가져옴
        public T GetSession()
        {
            lock (_lock)
            {
                if (_sessionPool.Count > 0)
                {
                    return _sessionPool.Pop();
                }
            }

            // 풀에 세션이 없으면 새로 생성하여 반환
            return new T();
        }

        // 세션을 풀에 반납
        public void ReturnSession(T session)
        {
            lock (_lock)
            {
                _sessionPool.Push(session);
            }
        }
    }

}

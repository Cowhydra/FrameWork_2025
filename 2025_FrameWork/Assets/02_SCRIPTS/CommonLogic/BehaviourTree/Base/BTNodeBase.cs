using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class BTNodeBase : BTElementBase
{
    protected List<BTNodeBase> _children = new List<BTNodeBase>();
    protected List<BTDecoratorBase> _decorators = new List<BTDecoratorBase>(); //� ��ҵ�

    /*
    서비스는 보통 주기적인 작업을 처리하므로, 자식 노드가 실행될 때마다 서비스가 업데이트되어야 합니다. 
    그렇지 않으면 게임 상태나 AI의 행동에 중요한 변경 사항이 반영되지 않을 수 있습니다.
    AI의 상태 업데이트: AI가 수행할 작업을 결정하는 데 필요한 상태(예: 체력, 위치, 목표)를 업데이트합니다.
타이머 관리: 일정 시간이 지난 후 작업을 수행하거나, 특정 조건을 만족했을 때 트리 내 다른 노드가 수행되도록 합니다.
조건 체크: 플레이어가 AI의 범위 내에 들어왔는지, 또는 자원을 수집했는지 등의 조건을 체크하고 트리의 상태를 갱신합니다.
    */
    protected List<BTServiceBase> _services = new List<BTServiceBase>(); //��忡 ���� �ΰ���ɵ�~

    //����
    protected System.Func<BehaviorTree.ENodeStatus> onEnterFn;

    //������Ʈ
    protected System.Func<BehaviorTree.ENodeStatus> onTickFunc;

    public BehaviorTree.ENodeStatus LastStatus { get; protected set; } = BehaviorTree.ENodeStatus.Unknown;
    public bool DecoratorsPermitRunning { get; protected set; } = true; //���ڷ����� �� ���, ���� ���� ���θ� ����.

    //������
    public BTNodeBase(string _Name = "", System.Func<BehaviorTree.ENodeStatus> _OnEnterFn = null,
    System.Func<BehaviorTree.ENodeStatus> _OnTickFn = null)
    {
        Name = _Name;
        onEnterFn = _OnEnterFn;
        onTickFunc = _OnTickFn;
    }


    //Node �߰��ϱ�
    public BTNodeBase Add<T>(string _Name,
        System.Func<BehaviorTree.ENodeStatus> _OnEnterFn = null,
        System.Func<BehaviorTree.ENodeStatus> _OnTickFn = null) where T : BTNodeBase, new()
    {
        T newNode = new T
        {
            Name = _Name,
            onEnterFn = _OnEnterFn,
            onTickFunc = _OnTickFn
        };

        return Add(newNode);
    }


    public BTNodeBase Add<T>(T newNode) where T : BTNodeBase
    {
        _children.Add(newNode);
        return newNode;
    }


    public BTNodeBase AddService<T>(string _Name, System.Action<float> _OnTickFn) where T : BTServiceBase, new()
    {
        T newService = new T();
        newService.Init(_Name, _OnTickFn);

        _services.Add(newService);

        return this;
    }


    public BTNodeBase AddService<T>(T newService) where T : BTServiceBase
    {
        _services.Add(newService);

        return this;
    }


    public BTNodeBase AddDecorator<T>(string _Name, System.Func<bool> _OnEvaluateFn) where T : BTDecoratorBase, new()
    {
        T newDecorator = new T();
        newDecorator.Init(_Name, _OnEvaluateFn);

        _decorators.Add(newDecorator);

        return this;
    }


    public BTNodeBase AddDecorator<T>(T newDecorator) where T : BTDecoratorBase
    {
        _decorators.Add(newDecorator);

        return this;
    }


    private void TickServices(float deltaTime)
    {
        foreach (var service in _services)
        {
            service.OnTick(deltaTime);
        }
    }


    private bool EvaluateDecorators()
    {
        bool canRun = true;

        foreach (var decorator in _decorators)
        {
            canRun = decorator.Evaluate();
            if (!canRun)
            {
                break;
            }
        }

        //���� ���¿�, ��� ����� �ٸ����
        if (canRun != DecoratorsPermitRunning)
        {
            //���� -> ���� or ���� -> ���� 
            DecoratorsPermitRunning = canRun;

            //���� ����->�����ΰ��
            //�ٽ� �ʱ� ���¿��� �����ؾ� �Ѵ�.
            //���� ���� ���³� �߰� ���� ���¸� �״�� �����ϸ� ������ ������ �߻��� �� �ֱ� �����Դϴ�.
            //���� ���� �Ǵ� �߰� ���� ���°� �״�� �����Ǿ� ��尡 ������������ ����.
            if (canRun)
            {
                Reset();
            }
        }

        return canRun;
    }


    protected virtual void OnAbort()
    {
        Reset();
    }


    public virtual void Reset()
    {
        LastStatus = BehaviorTree.ENodeStatus.Unknown;

        foreach (var child in _children)
        {
            child.Reset();
        }
    }


    public void Tick(float deltaTime)
    {
        bool tickedAnyNodes = OnTickInner(deltaTime);
        if (tickedAnyNodes == false)
        {
            Reset();
        }
    }


    protected virtual void OnEnter()
    {
        if (onEnterFn != null)
        {
            LastStatus = onEnterFn.Invoke();
        }
        else
        {
            LastStatus = _children.Count > 0 ? BehaviorTree.ENodeStatus.InProgress : BehaviorTree.ENodeStatus.Succeeded;
        }
    }


    protected virtual bool OnTickInner(float deltaTime)
    {
        //노드가 실행되었는지?
        bool tickedAnyNodes = false;

        // Decorators가 실행을 허용하지 않으면 실패 처리 후 종료
        if (DecoratorsPermitRunning == false)
        {
            LastStatus = BehaviorTree.ENodeStatus.Failed;// Decorators의 조건 불충족으로 실패 처리
            tickedAnyNodes = true;
            return tickedAnyNodes;
        }

        // 서비스 로직 실행
        TickServices(deltaTime);

        // 노드가 처음 실행되는 경우, OnEnter로 초기화 작업 수행
        if (LastStatus == BehaviorTree.ENodeStatus.Unknown)
        {
            OnEnter(); // OnEnter는 노드의 초기화 작업 및 상태 설정을 담당 --> 내부에서 LastStauts설정됨
            if (LastStatus == BehaviorTree.ENodeStatus.Failed)
            {
                return true; // 초기화 과정에서 실패하면 즉시 종료
            }
        }


        // Tick 함수 실행
        if (onTickFunc != null)
        {
            // onTickFunc에서 반환된 상태로 갱신
            LastStatus = onTickFunc.Invoke();
            tickedAnyNodes = true;

            // 작업이 완료된 상태면 종료
            if (LastStatus != BehaviorTree.ENodeStatus.InProgress)
            {
                return tickedAnyNodes;
            }
        }

        //틱 함수가 없거나, 노드상태가 계속 실행중이면 이 아래로 들어온다.
        // 자식 노드가 없을 경우 성공 처리 후 종료
        if (_children.Count == 0)
        {
            if (onTickFunc == null)
            {
                LastStatus = BehaviorTree.ENodeStatus.Succeeded;
            }

            tickedAnyNodes = false;
            return tickedAnyNodes;// 자식이 없으므로 Tick 수행은 없음
        }


        // 자식 노드 순회 처리
        for (int childIndex = 0; childIndex < _children.Count; ++childIndex)
        {
            //현재 내 child
            var child = _children[childIndex];

            // Decorators의 이전 상태와 현재 상태를 비교
            bool wasRunning = child.DecoratorsPermitRunning; // 이전 Tick에서 실행 가능 여부
            bool isRunning = child.EvaluateDecorators();     // 현재 Tick에서 실행 가능 여부


            // 자식 노드가 실행 중이거나, 초기 상태(Unknown)라면 Tick 수행
            if (child.LastStatus == BehaviorTree.ENodeStatus.InProgress || child.LastStatus == BehaviorTree.ENodeStatus.Unknown)
            {
                if (child.OnTickInner(deltaTime)) // 자식의 Tick 수행
                {
                    return true; // Tick이 수행되었음을 반환
                }

                // 자식의 최종 상태를 부모 노드의 상태로 업데이트
                LastStatus = child.LastStatus;

                // Decorators 상태가 변경된 경우 후속 노드 처리
                //불가능 -> 가능 ( 새롭게 갱신해줘야함 ), 가능->불가능 등의 경우는 굳이 처리 X 틱
                if (!wasRunning && isRunning)
                {
                    for (int nextIndex = childIndex + 1; nextIndex < _children.Count; ++nextIndex)
                    {
                        var afterchild = _children[nextIndex];
                        if (afterchild.LastStatus == BehaviorTree.ENodeStatus.InProgress)
                        {
                            afterchild.OnAbort();
                        }
                        else
                        {
                            afterchild.Reset();
                        }
                    }
                }


                // 자식 노드가 실패했고, 실패 시 평가 중단 조건을 충족하면 종료
                if (child.LastStatus == BehaviorTree.ENodeStatus.Failed &&
                    !ContinueEvaluatingIfChildFailed())
                {
                    return true; // 평가 중단, Tick 수행됨
                }

                // 자식 노드가 성공했고, 성공 시 평가 중단 조건을 충족하면 종료
                if (child.LastStatus == BehaviorTree.ENodeStatus.Succeeded &&
                    !ContinueEvaluatingIfChildSucceeded())
                {
                    return true; // 평가 중단, Tick 수행됨
                }
            }
        }


        //모든 자식 노드가 완료되었음
        OnTickedAllChildren();

        //더이상 틱을 실행하지 않음
        return false;
    }


    //���� �Ŀ��� ��� ���ϴ���?
    protected virtual bool ContinueEvaluatingIfChildFailed()
    {
        return true;
    }


    //���� �Ŀ��� ��� ���ϴ���?
    protected virtual bool ContinueEvaluatingIfChildSucceeded()
    {
        return true;
    }


    protected virtual void OnTickedAllChildren()
    {

    }


    public string GetDebugText()
    {
        StringBuilder debugTextBuilder = new StringBuilder();
        GetDebugTextInternal(debugTextBuilder);
        return debugTextBuilder.ToString();
    }


    protected override void GetDebugTextInternal(StringBuilder debugTextBuilder, int indentLevel = 0)
    {
        for (int index = 0; index < indentLevel; ++index)
        {
            debugTextBuilder.Append(' ');
        }

        debugTextBuilder.Append($"{Name} [{LastStatus.ToString()}]");

        foreach (var service in _services)
        {
            debugTextBuilder.AppendLine();
            debugTextBuilder.Append(service.GetDebugText(indentLevel + 1));
        }

        foreach (var decorator in _decorators)
        {
            debugTextBuilder.AppendLine();
            debugTextBuilder.Append(decorator.GetDebugText(indentLevel + 1));
        }

        foreach (var child in _children)
        {
            debugTextBuilder.AppendLine();
            child.GetDebugTextInternal(debugTextBuilder, indentLevel + 2);
        }
    }

}

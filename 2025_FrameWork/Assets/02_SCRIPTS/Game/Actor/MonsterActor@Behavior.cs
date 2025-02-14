using UnityEngine;

public partial class MonsterActor : BaseActor
{
    private StateMachine _MonsterAI;
    public bool IsMoving = false;

    private float moveSpeed => _MonsterData.MoveSpeed;
    private float chaseRange => _MonsterData.ChaseRange;
    private float attackpower => _MonsterData.AttackPower;
    private float attackRange => _MonsterData.AttackRange;
    private float detectRange => _MonsterData.DetectionRange;

    public D_F_Enum.E_ATTACK_TYPE attackType => _MonsterData.AttackType;


    private void Awake()
    {
        InitializeStateMachine();
    }


    private void InitializeStateMachine()
    {
        if (_MonsterAI == null)
        {
            _MonsterAI = new StateMachine();
        }

        MonsterStateNode_Idle idleState = new MonsterStateNode_Idle("Idle", this);
        MonsterStateNode_Detect detectState = new MonsterStateNode_Detect("Detect", this);
        MonsterStateNode_Chase chaseState = new MonsterStateNode_Chase("Chase", this);
        MonsterStateNode_Attack attackState = new MonsterStateNode_Attack("Attack", this);

        idleState.Transitions.Add(new StateTransition(detectState, FindTarget));
        detectState.Transitions.Add(new StateTransition(chaseState, IsTargetDetected));
        chaseState.Transitions.Add(new StateTransition(attackState, IsInAttackRange));
        attackState.Transitions.Add(new StateTransition(chaseState, IsOutAttackRange));

        _MonsterAI.SetInitialState(idleState);
    }


    private bool FindTarget()
    {
        if (TargetActor == null)
        {
            return false;
        }

        if (Vector3.SqrMagnitude(transform.position - TargetActor.GetCord) < detectRange * detectRange)
        {
            return true;
        }

        return false;
    }


    private bool IsTargetDetected()
    {
        if (TargetActor == null)
        {
            return false;
        }

        if (Vector3.SqrMagnitude(transform.position - TargetActor.GetCord) < chaseRange * chaseRange)
        {
            return true;
        }

        return false;
    }


    private bool IsInAttackRange()
    {
        if (TargetActor == null)
        {
            return false;
        }

        if (Vector3.SqrMagnitude(transform.position - TargetActor.GetCord) < attackRange * attackRange)
        {
            return true;
        }

        return false;
    }


    private bool IsOutAttackRange()
    {
        if (TargetActor == null)
        {
            return true;
        }

        if (Vector3.SqrMagnitude(transform.position - TargetActor.GetCord) < attackRange * attackRange)
        {
            return false;
        }

        return true;
    }


    public void Move(Vector3 movePos)
    {
        // 현재 위치에서 목표 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, movePos, _MonsterData.MoveSpeed * Time.deltaTime);

        // 목표 위치에 도달했는지 확인
        if (Vector3.Distance(transform.position, movePos) < 0.1f)
        {
            IsMoving = false; // 이동 멈춤
        }
        else
        {
            IsMoving = true;
        }
    }
}


public class MonsterStateNode_Idle : StateNode
{
    private MonsterActor _Owner;
    public MonsterStateNode_Idle(string name,MonsterActor owner) : base(name)
    {
        _Owner = owner;
    }


    public override void Enter()
    {
        base.Enter();

        if (_Owner != null)
        {
            _Owner.PlayAnimator(ActorActionID.Idle);
        }
    }
}


public class MonsterStateNode_Detect : StateNode
{
    private MonsterActor _Owner;
    public MonsterStateNode_Detect(string name, MonsterActor owner) : base(name)
    {
        _Owner = owner;
    }


    public override void Enter()
    {
        base.Enter();

        if (_Owner != null)
        {
            _Owner.PlayAnimator(ActorActionID.DETECT);
        }
    }
}


public class MonsterStateNode_Chase : StateNode
{
    private MonsterActor _Owner;
    public MonsterStateNode_Chase(string name, MonsterActor owner) : base(name)
    {
        _Owner = owner;
    }


    public override void Enter()
    {
        base.Enter();

        if (_Owner != null)
        {
            _Owner.PlayAnimator(ActorActionID.Chase);
        }
    }

    public override void Update()
    {
        base.Update();

        if (_Owner.TargetActor != null)
        {
            _Owner.Move(_Owner.TargetActor.GetCord);
        }
    }
}


public class MonsterStateNode_Attack : StateNode
{
    private MonsterActor _Owner;
    public MonsterStateNode_Attack(string name, MonsterActor owner) : base(name)
    {
        _Owner = owner;
    }


    public override void Enter()
    {
        base.Enter();

        if (_Owner != null)
        {
            _Owner.PlayAnimator(ActorActionID.Attack);
        }
    }
}

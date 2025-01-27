using D_F_Enum;
using UnityEngine;

public class PlayerActor : BaseActor
{
    [SerializeField] protected float _moveSpeed=10f;
    public override  E_OBJECT_TYPE ObjectType => E_OBJECT_TYPE.PLAYER;

    private int _exp;
    public int EXP
    {
        get
        {
            return _exp;
        }
        set
        {
            _exp = value;
        }
    }
}

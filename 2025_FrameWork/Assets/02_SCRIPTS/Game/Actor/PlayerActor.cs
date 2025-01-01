using D_F_Enum;
using UnityEngine;

public class PlayerActor : BaseActor
{
    public override E_OBJECT_TYPE ObjectType => E_OBJECT_TYPE.PLAYER;

    public bool IsMyPlayer => true;


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

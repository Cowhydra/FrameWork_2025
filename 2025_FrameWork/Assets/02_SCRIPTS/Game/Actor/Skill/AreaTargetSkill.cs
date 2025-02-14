using UnityEngine;

public class AreaTargetSkill : BaseSkill
{
    protected override void Hit()
    {
        if (_Owner == null)
        {
            return;
        }

        _Owner.T_HIT_INFO_FOR_AREA_MULTI_TARGET(DisTance, MaxEnmey, _Pos);
    }
}

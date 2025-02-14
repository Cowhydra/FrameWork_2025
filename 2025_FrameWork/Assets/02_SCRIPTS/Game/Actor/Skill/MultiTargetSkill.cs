using UnityEngine;

public class MultiTargetSkill : BaseSkill
{
    protected override void Hit()
    {
        if (_Owner == null)
        {
            return;
        }

        _Owner.T_HIT_INFO_FOR_MULTI_TARGET(DisTance, MaxEnmey);
    }
}

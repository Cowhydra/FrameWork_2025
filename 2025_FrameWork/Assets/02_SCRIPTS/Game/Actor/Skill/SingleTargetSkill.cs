public class SingleTargetSkill : BaseSkill
{
    protected override void Hit()
    {
        if (_Owner == null)
        {
            return;
        }

        _Owner.T_HIT_INFO_FOR_ONE_TARGET();
    }
}

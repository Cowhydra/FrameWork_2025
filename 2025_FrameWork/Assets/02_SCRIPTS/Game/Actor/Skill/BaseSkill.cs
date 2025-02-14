using UnityEngine;
//클라는 데미지 계산이 필요 없음
public abstract class BaseSkill
{
    public int SkillIndex { get; private set; }
    protected float Duration => _SkillData.Duration;
    protected float Cooldown => _SkillData.Cooldown;
    protected int[] HitFrames => _SkillData.HitFrames;
    protected int TotalHitCount => _SkillData.TotalHitCount;
    protected int DisTance => _SkillData.DisTance;
    protected int MaxEnmey => _SkillData.MaxAttackEnemy;

    private SkillData _SkillData;
    protected float _CurFrame = 0;

    protected int _CurHitNum = 0;
    protected bool _IsActive = false;
    protected bool _IsHitState = false;
    protected BaseActor _Owner;
    protected Vector2 _Pos;
    //스킬의 실행자 및 실행 위치 

    public virtual void Init(int skillIndex, BaseActor owner, Vector3 pos)
    {
        _IsActive = false;
        _CurHitNum = 0;
        SkillIndex = skillIndex;
        _Owner = owner;
        _Pos = pos;
        AssetServer.SkillDataDict.Value.TryGetValue(skillIndex, out _SkillData);
    }

    public void ResetSkill()
    {
        _CurFrame = 0;
    }


    public void Deactivate()
    {
        _IsActive = false;
    }


    public void UpdateSkill(float deltaTime)
    {
        if(_SkillData == null)
        {
            return;
        }

        if (_IsHitState == true)
        {
            return;
        }

        if (_IsActive == false)
        {
            return;
        }

        _CurFrame += deltaTime;

        foreach (int hitFrame in HitFrames)
        {
            if ((int)_CurFrame == hitFrame)
            {
                if(_CurHitNum< TotalHitCount)
                {
                    _CurHitNum++;
                    Hit();
                }
            }
        }

        _IsHitState = false;

        if (_CurFrame >= Duration)
        {
            SkillPooll.ReturnSkill(this);
        }
    }

    
    protected abstract void Hit();
}

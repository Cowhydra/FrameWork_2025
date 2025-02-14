using System.Collections.Generic;

public static class SkillPooll
{
    private static Dictionary<int, Queue<BaseSkill>> _skillPools = new Dictionary<int, Queue<BaseSkill>>();

    public static BaseSkill GetSkill(int skillIndex)
    {
        if (_skillPools.TryGetValue(skillIndex, out Queue<BaseSkill> skillQueue) && skillQueue.Count > 0)
        {
            BaseSkill skill = skillQueue.Dequeue();
            skill.ResetSkill(); // 스킬 초기화
            return skill;
        }

        // 풀에 없으면 새로 생성
        BaseSkill newSkill = CreateNewSkill(skillIndex);
        return newSkill;
    }


    public static void ReturnSkill(BaseSkill skill)
    {
        if (_skillPools.ContainsKey(skill.SkillIndex)==false)
        {
            _skillPools[skill.SkillIndex] = new Queue<BaseSkill>();
        }

        skill.Deactivate(); // 비활성화 처리
        _skillPools[skill.SkillIndex].Enqueue(skill);
    }


    private static BaseSkill CreateNewSkill(int skillIndex)
    {
        if(AssetServer.SkillDataDict.Value.TryGetValue(skillIndex,out SkillData skillData) == false)
        {
            return null;
        }

        //패시브는 일단..클라에서 따로 공격 처리해줄 필요가 없다
        switch (skillData.SkillType)
        {
            case D_F_Enum.E_SKILL_TYPE.NONE:
            case D_F_Enum.E_SKILL_TYPE.PASSIVE:
                return null;
            case D_F_Enum.E_SKILL_TYPE.SINGLE:
                return new SingleTargetSkill();
            case D_F_Enum.E_SKILL_TYPE.MULTI:
                return new MultiTargetSkill();
            case D_F_Enum.E_SKILL_TYPE.AREA:
                return new AreaTargetSkill();
        }

        return null;
    }
}

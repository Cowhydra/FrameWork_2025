using System.Collections.Generic;

public static class SkillPooll
{
    private static Dictionary<int, Queue<BaseSkill>> _skillPools = new Dictionary<int, Queue<BaseSkill>>();

    public static BaseSkill GetSkill(int skillIndex)
    {
        if (_skillPools.TryGetValue(skillIndex, out Queue<BaseSkill> skillQueue) && skillQueue.Count > 0)
        {
            BaseSkill skill = skillQueue.Dequeue();
            skill.ResetSkill(); // ��ų �ʱ�ȭ
            return skill;
        }

        // Ǯ�� ������ ���� ����
        BaseSkill newSkill = CreateNewSkill(skillIndex);
        return newSkill;
    }


    public static void ReturnSkill(BaseSkill skill)
    {
        if (_skillPools.ContainsKey(skill.SkillIndex)==false)
        {
            _skillPools[skill.SkillIndex] = new Queue<BaseSkill>();
        }

        skill.Deactivate(); // ��Ȱ��ȭ ó��
        _skillPools[skill.SkillIndex].Enqueue(skill);
    }


    private static BaseSkill CreateNewSkill(int skillIndex)
    {
        if(AssetServer.SkillDataDict.Value.TryGetValue(skillIndex,out SkillData skillData) == false)
        {
            return null;
        }

        //�нú�� �ϴ�..Ŭ�󿡼� ���� ���� ó������ �ʿ䰡 ����
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

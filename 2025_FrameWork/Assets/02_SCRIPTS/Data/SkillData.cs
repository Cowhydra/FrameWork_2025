using D_F_Enum;

[System.Serializable]
public class SkillData
{
    public readonly int SkillIndex;        // ��ų �ε���
    public readonly float Duration;        // ��ų ���ӽð�
    public readonly int TotalHitCount;     // �� ��Ʈ ��
    public readonly int[] HitFrames;       // ��Ʈ ��ų ������
    public readonly float DamagePercent;   // ������ %
    public readonly float ArmorPercent;    // �� %
    public readonly E_SKILL_TYPE SkillType;  // PASSIVE(1) or SINGLE(2) or MULTI(3) or AREA(4)
    public readonly int ManaCost;          // ���� �Ҹ�
    public readonly float Cooldown;        // ���� ���ð�
    public readonly bool IsProjectile;     // ����ü ��� ����
    public readonly int ProjectileID;      // ����ü ID (��� �� �ϸ� -1)
    public readonly int ActionID;          // ���� ��ȣ
    public readonly int DisTance;
    public readonly int MaxAttackEnemy = 5;

    public SkillData(int skillIndex, string[] fields)
    {
        SkillIndex = skillIndex;
        Duration = float.Parse(fields[1]);
        TotalHitCount = int.Parse(fields[2]);
        HitFrames = ParseHitFrames(fields[3]);
        DamagePercent = float.Parse(fields[4]);
        ArmorPercent = float.Parse(fields[5]);
        SkillType = (E_SKILL_TYPE)int.Parse(fields[6]);
        ManaCost = int.Parse(fields[7]);
        Cooldown = float.Parse(fields[8]);
        IsProjectile = bool.Parse(fields[9]);
        ProjectileID = int.Parse(fields[10]);
        ActionID = int.Parse(fields[11]);
        DisTance = int.Parse(fields[12]);

    }

    int[] ParseHitFrames(string hitFramesField)
    {
        if (string.IsNullOrWhiteSpace(hitFramesField))
        {
            return new int[0];
        }

        string[] frames = hitFramesField.Split(';');
        int[] hitFrames = new int[frames.Length];

        for (int i = 0; i < frames.Length; i++)
        {
            hitFrames[i] = int.Parse(frames[i]);
        }

        return hitFrames;
    }
}

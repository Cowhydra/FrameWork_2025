using D_F_Enum;

[System.Serializable]
public class SkillData
{
    public readonly int SkillIndex;        // 스킬 인덱스
    public readonly float Duration;        // 스킬 지속시간
    public readonly int TotalHitCount;     // 총 히트 수
    public readonly int[] HitFrames;       // 히트 스킬 프레임
    public readonly float DamagePercent;   // 데미지 %
    public readonly float ArmorPercent;    // 방어구 %
    public readonly E_SKILL_TYPE SkillType;  // PASSIVE(1) or SINGLE(2) or MULTI(3) or AREA(4)
    public readonly int ManaCost;          // 마나 소모량
    public readonly float Cooldown;        // 재사용 대기시간
    public readonly bool IsProjectile;     // 투사체 사용 여부
    public readonly int ProjectileID;      // 투사체 ID (사용 안 하면 -1)
    public readonly int ActionID;          // 동작 번호
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

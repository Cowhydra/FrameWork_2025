using D_F_Enum;

public class MonsterData 
{
    public readonly int UniqueKey;
    public readonly int Health;
    public readonly int Mana;
    public readonly int AttackPower;
    public readonly float AttackRange;
    public readonly float MoveSpeed;
    public readonly float DetectionRange;
    public readonly float ChaseRange;
    public readonly int ModelNumber;
    public readonly int[] DropItem;
    public readonly E_ATTACK_TYPE AttackType;     // 0, 1, 2 타입
    public readonly int ProjectileID;   // 발사체 번호

    public MonsterData(int UniqueKey,string[] fields)
    {
        UniqueKey = this.UniqueKey;
        Health = int.Parse(fields[1]);
        Mana = int.Parse(fields[2]);
        AttackPower = int.Parse(fields[3]);
        AttackRange = float.Parse(fields[4]);
        MoveSpeed = float.Parse(fields[5]);
        DetectionRange = float.Parse(fields[6]);
        ChaseRange = float.Parse(fields[7]);
        ModelNumber = int.Parse(fields[8]);
        DropItem = ParseDropItems(fields[9]);
        AttackType = (E_ATTACK_TYPE)int.Parse(fields[10]);
        ProjectileID = int.Parse(fields[11]);
    }


    int[] ParseDropItems(string dropItemsField)
    {
        if (string.IsNullOrWhiteSpace(dropItemsField))
        {
            return new int[0];
        }

        string[] items = dropItemsField.Split(';'); // 세미콜론으로 분리
        int[] dropItems = new int[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            dropItems[i] = int.Parse(items[i]);
        }

        return dropItems;
    }
}

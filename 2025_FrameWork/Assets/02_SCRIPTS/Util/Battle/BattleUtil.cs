using UnityEngine;

public static class BattleUtil 
{
   public static bool GetSuccessFor_ONE_M(int num)
    {
        return Random.Range(0, 10000) <= num;
    }
}

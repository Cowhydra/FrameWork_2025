using D_F_Enum;
using UnityEngine;

public partial class GameLogic : MonoBehaviour
{
    /// <summary>
    /// 히트 규칙
    /// </summary>
    /// <param name="hitUser"></param>
    /// <param name="beatenUser"></param>
    /// <param name="hitSort"></param>
    /// <param name="beatenSort"></param>
    public void HIT(E_OBJECT_TYPE hitOjbectType, int hitUser, int actionID, E_OBJECT_TYPE targetOjbectType, int beatenUser, E_HIT_SORT hitSort, int damage)
    {
        //플레이어를 떄릴 경우
        if(hitOjbectType is E_OBJECT_TYPE.PLAYER)
        {
            PlayerActor hitter = null;
            PlayerActor beater = null;

            if(FindPlayer(hitUser, out hitter) && FindPlayer(beatenUser,out beater) == true)
            {
                bool isCritical = BattleUtil.GetSuccessFor_ONE_M(500);
                if (isCritical)
                {
                    beater.R_BEATEN_INFO(E_BEATEN_SORT.NORMAL, damage, hitter);
                }
                else
                {
                    beater.R_BEATEN_INFO(E_BEATEN_SORT.NORMAL, damage, hitter);
                }
            }

        }
        else
        {

        }
    }
}

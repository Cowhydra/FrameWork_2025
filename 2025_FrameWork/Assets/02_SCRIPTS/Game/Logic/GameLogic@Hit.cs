using D_F_Enum;
using UnityEngine;

public partial class GameLogic : MonoBehaviour
{

    //일단 네트워크 X 


    /// <summary>
    /// 히트 규칙
    /// </summary>
    /// <param name="hitUser"></param>
    /// <param name="beatenUser"></param>
    /// <param name="hitSort"></param>
    /// <param name="beatenSort"></param>
    public void HIT(E_OBJECT_TYPE hitterOjbectType, int hitUser, int attackID, E_OBJECT_TYPE targetOjbectType, int beatenUser, E_HIT_SORT hitSort, int damage)
    {
        //플레이어를 떄릴 경우
        if(hitterOjbectType is E_OBJECT_TYPE.PLAYER)
        {
            PlayerActor hitter = null;
            PlayerActor beater = null;

            if(FindPlayer(hitUser, out hitter) && FindPlayer(beatenUser,out beater) == true)
            {
                beater.R_BEATEN_INFO(E_BEATEN_SORT.NORMAL, damage, hitter);
            }
        }
        else
        {

        }
    }
}

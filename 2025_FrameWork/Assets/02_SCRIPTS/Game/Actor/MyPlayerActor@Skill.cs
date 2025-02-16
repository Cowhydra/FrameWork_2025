using UnityEngine;

public partial class MyPlayerActor : PlayerActor
{
   private Vector3 TargetPos { get; set; } = Vector3.zero;

    //현재 스킬이라는 행위 자체만 가지고 있음


    //스킬 사용 시 고려대상
   public void EXCUTE_SKILL(int skillNumber)
    {
        if (skillNumber == 0)
        {
            return;
        }

        if(AssetServer.SkillDataDict.Value.TryGetValue(skillNumber, out var skillData) == false)
        {
            return;
        }

        //이펙트 


        //액션
        

        //스킬
        BaseSkill currSKill = SkillPooll.GetSkill(skillNumber);
        currSKill.Init(skillNumber, this, TargetPos);
    }


    //마우스 등으로 TargetPos 를 설정해줘야함 --> 만약 마우스 등으로 Target Pos를 설정할 것이라면..

}

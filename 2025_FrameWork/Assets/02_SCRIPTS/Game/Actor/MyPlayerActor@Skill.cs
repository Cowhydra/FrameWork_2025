using UnityEngine;

public partial class MyPlayerActor : PlayerActor
{
   private Vector3 TargetPos { get; set; } = Vector3.zero;

    //���� ��ų�̶�� ���� ��ü�� ������ ����


    //��ų ��� �� ������
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

        //����Ʈ 


        //�׼�
        

        //��ų
        BaseSkill currSKill = SkillPooll.GetSkill(skillNumber);
        currSKill.Init(skillNumber, this, TargetPos);
    }


    //���콺 ������ TargetPos �� ����������� --> ���� ���콺 ������ Target Pos�� ������ ���̶��..

}

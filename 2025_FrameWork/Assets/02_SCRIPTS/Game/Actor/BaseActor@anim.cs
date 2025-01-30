using System.Collections.Generic;
using UnityEngine;

public partial class BaseActor : MonoBehaviour
{
    //모든 행동은 강제플레이가 아니면 아이돌로
    private Animator _animator;

    public async void PlayAnimator(string anim)
    {
        if (_animator == null)
        {
            return;
        }
    
        if (_animator.runtimeAnimatorController == null)
        {
            string path = GetAnimationPath(anim);

            RuntimeAnimatorController rc =await AssetServer.LoadAsync<RuntimeAnimatorController>(anim);

            if (rc != null)
            {
                _animator.runtimeAnimatorController = rc;
            }
        }

        _animator.Play(anim);
    }


    private void UpdateForceIdle()
    {
        if (_animator == null)
        {
            return;
        }

        // Idle 애니메이션이 필요하면 실행
        PlayAnimator(ActorActionID.Idle);
    }


    private string GetAnimationPath(string anim)
    {
        if (ObjectType == D_F_Enum.E_OBJECT_TYPE.MONSTER)
        {
            return $"animatonclip/enemy/enemy_{UniqueIndex:D3}/enemy_{UniqueIndex:D3}animcontroller.controller";
        }
        else if (ObjectType == D_F_Enum.E_OBJECT_TYPE.PLAYER)
        {
            return $"animatonclip/player/playeranimcontroller.controller";
        }

        return string.Empty;
    }


    private void UpdateAnimation()
    {
        //TEMP
        UpdateForceIdle();
    }
}

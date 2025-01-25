using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public partial class BaseActor : MonoBehaviour
{
    //��� �ൿ�� �����÷��̰� �ƴϸ� ���̵���
    private Animator _animator;

    public void PlayAnimator(string anim)
    {
        if (_animator == null)
        {
            return;
        }
    
        if (_animator.runtimeAnimatorController == null)
        {
            string path = GetAnimationPath(anim);

            if (AssetServer.GetAnimatorController(anim,out var controller) == false)
            {
                return;
            }

            _animator.runtimeAnimatorController= controller;
        }

        _animator.Play(anim);
    }


    private void UpdateForceIdle()
    {
        if (_animator == null)
        {
            return;
        }

        // Idle �ִϸ��̼��� �ʿ��ϸ� ����
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

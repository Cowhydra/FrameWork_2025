using UnityEngine;

public partial class BaseActor : MonoBehaviour
{
    //모든 행동은 강제플레이가 아니면 아이돌로
    Animation _animation;

    public void PlayAnimator(string anim)
    {
        if (_animation == null)
        {
            return;
        }

        if (ObjectType == D_F_Enum.E_OBJECT_TYPE.MONSTER)
        {
            _animation.clip = AssetServer.Load<AnimationClip>($"animatonclip/enemy/enemy_{UniqueIndex:D3}/{anim}.anim");
        }
        else if (ObjectType == D_F_Enum.E_OBJECT_TYPE.PLAYER)
        {
            _animation.clip = AssetServer.Load<AnimationClip>($"animatonclip/player/{anim}.anim");
        }

        _animation.Play();
    }


    private void UpdateForceIdle()
    {
        if (_animation == null)
        {
            return;
        }

        if (_animation.isPlaying == false)
        {
            if(ObjectType== D_F_Enum.E_OBJECT_TYPE.MONSTER)
            {
                _animation.clip = AssetServer.Load<AnimationClip>($"animatonclip/enemy/enemy_{UniqueIndex:D3}/{ActorActionID.Idle}.anim");
            }
            else if (ObjectType == D_F_Enum.E_OBJECT_TYPE.PLAYER)
            {
                _animation.clip = AssetServer.Load<AnimationClip>($"animatonclip/player/{ActorActionID.Idle}.anim");
            }
        }

        if (_animation.clip != null)
        {
            _animation.Play();
        }
    }


    private void Update()
    {
        UpdateForceIdle();
    }
}

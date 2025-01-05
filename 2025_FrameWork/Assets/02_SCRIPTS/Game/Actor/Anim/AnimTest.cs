using Unity.VisualScripting;
using UnityEngine;

public class AnimTest : MonoBehaviour
{
    [SerializeField] private AnimationClip _Clip;
    private Animation _Anim;
    private void Awake()
    {
        TryGetComponent(out _Anim);

        if (_Anim.clip != null)
        {
            _Anim.Play();
        }
    }


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            //GameLogic.Instance.MyPlayer.PlayAnimator(ActorActionID.Walk);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
           // GameLogic.Instance.MyPlayer.PlayAnimator("Jump");
        }

    }
}

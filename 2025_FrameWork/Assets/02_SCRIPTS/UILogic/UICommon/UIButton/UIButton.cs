using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class UIButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum E_TRANSITION_TYPE
    {
        NONE,
        SCALE
    }

    public Sprite Disabled;

    public TextMeshProUGUI Label;
    public bool PlayDefClickSound = true;
    public AudioClip ClickSound;

    public E_TRANSITION_TYPE TransitionType = E_TRANSITION_TYPE.SCALE;

    public float ScaleOnDown = 0.95f; // PointDown시 Scale값
    public float ScaleSpeed = 10f;

    private bool _IsInit = false;

    private Sprite _Normal;
    private Image _Image;

    private Color _NormalColor;
    private float _CurScale = 1f;

    private bool _Interactable = true;
    private int _TransitionState = 0;

    private RectTransform _CachedTr;

    public System.Action onClick = null;

    private static readonly Color _DISALBED_COLOR = new Color(0.3f, 0.3f, 0.3f);


    private void Awake()
    {
        _IsInit = true;

        _CachedTr = GetComponent<RectTransform>();

        _Image = GetComponent<Image>();
        _Normal = _Image.sprite;

        _NormalColor = (Label != null ? Label.color : Color.white);

        // 주의: 이미 SetInteractable()이 실행돼서 Interactable이 false가 되어 있을 수 있음
        if (_Interactable == false)
        {
            SetInteractable(false);
        }
    }


    private void Update()
    {
        if (_Interactable == false)
        {
            return;
        }

        if (_TransitionState != 0)
        {
            DoTransition(Time.deltaTime);
        }
    }


    // 주의: SetInteractable()이 Awake()보다 먼저 호출될 수 있음.
    public void SetInteractable(bool val)
    {
        _Interactable = val;

        // 아직 Awake()가 실행되지 않은 경우
        if (_IsInit == false)
        {
            return;
        }

        if (_Interactable == false)
        {
            if (Disabled != null)
            { 
                _Image.sprite = Disabled;
            }

            if (Label != null)
            { 
                Label.color = _DISALBED_COLOR;
            }
        }
        else
        {
            _Image.sprite = _Normal;

            if (Label != null)
            {     
                Label.color = _NormalColor;
            }
        }
    }


    private void DoTransition(float deltaTime)
    {
        if (TransitionType == E_TRANSITION_TYPE.SCALE)
        {
            DoTransition_Scale(deltaTime);
        }
    }


    private void DoTransition_Scale(float deltaTime)
    {
        // OnPointDown
        if (_TransitionState == 1)
        {
            if (ScaleOnDown > 1f)
            {
                _CurScale += (deltaTime * ScaleSpeed);
                if (_CurScale > ScaleOnDown)
                {
                    _CurScale = ScaleOnDown;
                }
            }
            else
            {
                _CurScale -= (deltaTime * ScaleSpeed);
                if (_CurScale < ScaleOnDown)
                {
                    _CurScale = ScaleOnDown;
                }
            }
        }
        // OnPointUp
        else if (_TransitionState == 2)
        {
            if (ScaleOnDown > 1f)
            {
                _CurScale -= (deltaTime * ScaleSpeed);
                if (_CurScale < 1f)
                {
                    _CurScale = 1f;
                    _TransitionState = 0;
                }
            }
            else
            {
                _CurScale += (deltaTime * ScaleSpeed);
                if (_CurScale > 1f)
                {
                    _CurScale = 1f;
                    _TransitionState = 0;
                }
            }
        }

        _CachedTr.localScale = new Vector3(_CurScale, _CurScale, _CurScale);
    }


    private void ConditionalPlayClickSound()
    {
        if (SoundManager.IsValid == false)
        {
            return;
        }

        if (PlayDefClickSound == true)
        {
           // TSSoundManager.Instance.PLAY_UI_SOUND(TSSoundID.); //TODO:
        }
        else if (ClickSound != null)
        {
            SoundManager.Instance.PLAY_UI_SOUND(ClickSound);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (_Interactable == false)
        {
            return;
        }

        ConditionalPlayClickSound();

        if (onClick != null)
        {
            onClick.Invoke();
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (_Interactable == false || TransitionType == E_TRANSITION_TYPE.NONE)
        {
            return;
        }

        _TransitionState = 1;
    }

    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_Interactable == false || TransitionType == E_TRANSITION_TYPE.NONE)
        {
            return;
        }

        _TransitionState = 2;
    }
}

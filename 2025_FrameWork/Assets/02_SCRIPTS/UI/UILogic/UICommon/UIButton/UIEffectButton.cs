using UnityEngine;
using TMPro;


[RequireComponent(typeof(UIButton))]
[RequireComponent(typeof(UIEffectButtonColor))]
public class UIEffectButton : MonoBehaviour
{
    [Header("버튼")]
    protected UIButton _Button;
    private UIEffectButtonColor _ButtonColor;
    [Header("버튼 ON 상태일 떄 오브젝트")]
    [SerializeField] private GameObject[] _EffectOnObject;
    [Header("버튼 OFF 상태일 떄 오브젝트")]
    [SerializeField] private GameObject[] _EffectOffObject;
    [Header("버튼 라벨")]
    [SerializeField] protected TextMeshProUGUI _ButtonLabel;
    
    [Header("선택에 따른 컬러 효과")]
    private bool _Init = false;

    protected virtual void Awake()
    {
        if (_Init == false)
        {
            Init();
        }
    }


    public virtual void SetButton(UIEffectButtonGroup owner, System.Action clickaction)
    {
        if (_Init == false)
        {
            Init();
        }

        if (_Button != null)
        {
            _Button.onClick = null;
            _Button.onClick += () => NotifyGroup(owner);
            _Button.onClick += clickaction;
        }
    }


    public virtual void SetLabel(string value)
    {
        if (_ButtonLabel != null)
        {
            _ButtonLabel.text = value;
        }
    }



    private void NotifyGroup(UIEffectButtonGroup owner)
    {
        owner.NotiftyEffectOn(this);
    }


    public virtual void SetEffect(bool isOn)
    {
        if (_Init == false)
        {
            Init();
        }

        if (_EffectOnObject != null)
        {
            for (int i = 0, len = _EffectOnObject.Length; i < len; ++i)
            {
                if (_EffectOnObject[i] != null)
                {
                    _EffectOnObject[i].SetActive(isOn);
                }
            }
        }

        if (_EffectOffObject != null)
        {
            for (int i = 0, len = _EffectOffObject.Length; i < len; ++i)
            {
                if (_EffectOffObject[i] != null)
                {
                    _EffectOffObject[i].SetActive(!isOn);
                }
            }
        }

        if (_ButtonColor != null) 
        {
            if (_ButtonLabel != null)
            {
                if (_ButtonColor.UseOnOffColor() == true)
                {
                    if (isOn)
                    {
                        _ButtonLabel.color = _ButtonColor.GetOnColor();
                    }
                    else
                    {
                        _ButtonLabel.color = _ButtonColor.GetOffColor();
                    }
                }
             
            }
        }
    }


    private void Init()
    {
        TryGetComponent(out _Button);
        TryGetComponent(out _ButtonColor);
        if (_EffectOnObject != null)
        {
            for(int i = 0, len = _EffectOnObject.Length; i < len; ++i)
            {
                if (_EffectOnObject[i] != null)
                {
                    _EffectOnObject[i].SetActive(false);
                }
            }
        }

        if (_EffectOffObject != null)
        {
            for (int i = 0, len = _EffectOffObject.Length; i < len; ++i)
            {
                if (_EffectOffObject[i] != null)
                {
                    _EffectOffObject[i].SetActive(false);
                }
            }
        }

        if (_ButtonLabel != null)
        {
            if (_ButtonColor != null)
            {
                _ButtonLabel.color= _ButtonColor.GetOnColor();
            }
            else
            {
                _ButtonLabel.color = Color.white;
            }
        }

        _Init = true;
    }
}

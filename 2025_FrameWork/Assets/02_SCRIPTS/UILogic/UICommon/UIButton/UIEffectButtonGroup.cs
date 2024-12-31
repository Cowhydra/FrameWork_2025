using System.Collections.Generic;

public class UIEffectButtonGroup
{
    private List<UIEffectButton> _ButtonGroup;

    public UIEffectButtonGroup(int MaxGroup = 5)
    {
        _ButtonGroup = new List<UIEffectButton>(MaxGroup);
    }


    public int Count => _ButtonGroup.Count;


    public void RegisterButtonGroup(UIEffectButton button)
    {
        if (_ButtonGroup.Contains(button) == false)
        {
            _ButtonGroup.Add(button);
        }
    }


    public void UnRegisterButtonGroup(UIEffectButton button)
    {
        if (_ButtonGroup.Contains(button) == true)
        {
            _ButtonGroup.Remove(button);
        }
    }


    public void NotiftyEffectOn(UIEffectButton button)
    {
        ValidateButtonIsInGroup(button);

        for (int i = 0, len = _ButtonGroup.Count; i < len; ++i)
        {
            if (ReferenceEquals(_ButtonGroup[i], button) == true)  
            {
                _ButtonGroup[i].SetEffect(true);
            }
            else
            {
                _ButtonGroup[i].SetEffect(false);
            }
        }
    }


    public void RefreshGroupUI(int idx = 0)
    {
        if (_ButtonGroup.Count > idx && _ButtonGroup[idx] != null) 
        {
            NotiftyEffectOn(_ButtonGroup[idx]);
        }
    }


    public void ResetGroupUI()
    {
        if (_ButtonGroup != null)
        {
            for (int i = 0, len = _ButtonGroup.Count; i < len; ++i)
            {
                _ButtonGroup[i].SetEffect(false);
            }
        }
    }


    private void ValidateButtonIsInGroup(UIEffectButton button)
    {
        if (button == null || _ButtonGroup.Contains(button) == false)
        {
            throw new System.ArgumentException(" Button {0} is not part of ButtonGroup", button.gameObject.name);
        }
    }
}

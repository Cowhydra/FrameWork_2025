using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISliderMsg : MonoBehaviour
{
    [SerializeField] private Image _ProgressImg;
    [SerializeField] private TextMeshProUGUI _PercentLbl;
    [SerializeField] private TextMeshProUGUI _TitleLabel;


    public void InitSliderMsg(string msg)
    {
        if (_TitleLabel != null)
        {
            _TitleLabel.text = msg;
        }

        gameObject.SetActive(true);

        _TitleLabel.SetActiveEx(true);
        _PercentLbl.SetActiveEx(false);
        _TitleLabel.SetActiveEx(false);
    }


    public void ClearSliderMsg()
    {
        gameObject.SetActive(false);
    }



    public void ChangeSliderMsg(string msg)
    {
        if (_TitleLabel != null)
        {
            _TitleLabel.text = msg;
        }
    }


    // ÁøÇà·ü °»½Å
    public void UpdateProgress(float rate)
    {
        _ProgressImg.fillAmount = Mathf.Clamp01(rate);
        _PercentLbl.text = string.Format("{0}%", (int)(rate * 100f));

        _ProgressImg.SetActiveEx(true);
        _PercentLbl.SetActiveEx(true);
    }
}

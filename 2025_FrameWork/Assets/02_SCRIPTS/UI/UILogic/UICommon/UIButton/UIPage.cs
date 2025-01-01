using TMPro;
using UnityEngine;

public class UIPage : MonoBehaviour
{
    [SerializeField] private UIButton _PrevButton;
    [SerializeField] private UIButton _NextButton;
    [SerializeField] private TextMeshProUGUI _PageLabel;

    [SerializeField] private System.Action _PageChangeAction;

    private int _MaxPage = 1;
    private int _CurrPage = 1;

    public int MaxPage => _MaxPage;

    public int CurrPage => _CurrPage;

    private void Awake()
    {
        _PrevButton.onClick = OnPrevPageClick;
        _NextButton.onClick = OnNextPageClick;

        SetPageLabel();
    }


    public void SetPage(int maxpage, System.Action pageaction)
    {
        _CurrPage = 1;
        _MaxPage = maxpage;
        _PageChangeAction = pageaction;

        SetPageLabel();
    }


    //중간 데이터 갱신 때문 --EnhanceSccoller// 경매장 등
    public void SetPage(int currpage, int maxpage, System.Action pageaction)
    {
        _CurrPage = Mathf.Max(currpage, 1);
        _MaxPage = Mathf.Max(1, maxpage);

        if (_MaxPage < 1)
        {
            _MaxPage = 1;
        }

        if (_CurrPage >= _MaxPage)
        {
            _CurrPage = _MaxPage;
        }

        _PageChangeAction = pageaction;

        SetPageLabel();
    }



    private void OnNextPageClick()
    {
        if (_CurrPage >= _MaxPage)
        {
            _CurrPage = _MaxPage;
            SetPageLabel();

            return;
        }

        _CurrPage++;
        _PageChangeAction?.Invoke();

        SetPageLabel();
    }


    private void OnPrevPageClick()
    {
        if (_CurrPage <= 1)
        {
            _CurrPage = 1;
            SetPageLabel();
            return;
        }

        _CurrPage--;
        _PageChangeAction?.Invoke();

        SetPageLabel();
    }


    private void SetPageLabel()
    {
        if (_PageLabel != null)
        {
            if (_CurrPage >= _MaxPage)
            {
                _CurrPage = _MaxPage;
            }
            _PageLabel.text = $"{_CurrPage}/{_MaxPage}";
        }
    }
}

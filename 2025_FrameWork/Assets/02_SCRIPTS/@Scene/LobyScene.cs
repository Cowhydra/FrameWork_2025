using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobyScene : UIScene
{
    [Header("허드가 있어야함")]
    [SerializeField] private UIButton _GameStartBtn;

    private Coroutine _SceneMoveCorotine;

    protected override void Awake()
    {
        base.Awake();


        this.SafeStopCoroutine(ref _SceneMoveCorotine);

        if (_GameStartBtn != null)
        {
            _GameStartBtn.onClick = OnGameStartBtnClick;
        }
    }



    private void OnGameStartBtnClick()
    {
        _SceneMoveCorotine=StartCoroutine(nameof(GotoNextStage));
    }


    private IEnumerator GotoNextStage()
    {
        AsyncOperation async = SceneLoader.LoadZoneSceneAsync(GLOBAL_DATA.NextSceneName, LoadSceneMode.Additive);
        if (async == null)
        {
            OnError("FAIL_LOAD_ZONE");
            yield break;
        }

        while (!async.isDone)
        {
            yield return null;
        }

        SceneLoader.SetActiveScene(GLOBAL_DATA.NextSceneName);

        SceneLoader.LoadScene(D_F_Enum.SCENE_NAME.Game, LoadSceneMode.Additive);

        // Loading 씬 제거
        yield return SceneLoader.UnloadSceneAsync(D_F_Enum.SCENE_NAME.Lobby);
    }


    private void OnError(string errCode)
    {
        Current.ShowNoticeMsg(errCode);
    }
}

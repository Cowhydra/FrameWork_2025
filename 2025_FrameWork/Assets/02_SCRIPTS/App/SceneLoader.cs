using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static string NEXT_SCENE_NAME { get; private set; }


    // 파라미터 설정
    private static void SetParam(string nextSceneName)
    {
        NEXT_SCENE_NAME = nextSceneName;
    }


    // 씬 로딩 (동기)
    public static Scene LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        SetParam(string.Empty);

        return SceneManager.LoadScene(sceneName, new LoadSceneParameters(loadSceneMode));
    }


    // 씬 로딩 (비동기)
    public static AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        SetParam(string.Empty);

        return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
    }


    // 로딩 씬 로딩
    public static void LoadLoadingScene(string sceneName, string nextSceneName)
    {
        SetParam(nextSceneName);

        SceneManager.LoadScene(sceneName);
    }


    // 씬 활성
    public static void SetActiveScene(string sceneName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }


    // 씬 언로딩
    public static AsyncOperation UnloadSceneAsync(string sceneName)
    {
        return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
    }
}


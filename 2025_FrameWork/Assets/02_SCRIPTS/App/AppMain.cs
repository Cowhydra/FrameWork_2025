using System.Collections;
using D_F_Enum;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppMain : MonoBehaviour
{
    private IEnumerator Start()
    {
        // 스크린 해상도 설정 (기본: 1920 x 1080)
        yield return SetScreenResolution();

        SceneLoader.LoadScene(SCENE_NAME.Patch, LoadSceneMode.Single);
    }


    private IEnumerator SetScreenResolution()
    {
        // 디바이스 스크린 사이즈 얻기
        ResolutionUtil.GetDeviceScreenSize();

        // 주의: 1프레임 건너뛰어야 정확한 SafeArea 값을 얻을 수 있음.
        yield return null;

        // 디바이스 SafeArea 영역 얻기
        ResolutionUtil.GetDeviceSafeArea();
        yield return null;

        // 적당한 스크린 해상도를 얻고, 적용
        Vector2Int resolution = ResolutionUtil.CalcRenderResolution();
        ResolutionUtil.SetScreenResolution(resolution.x, resolution.y);

        // 주의: 1프레임 건너뛰어야 변경된 Screen 정보 값을 얻을 수 있음.
        yield return null;

        // 스크린 정보들 재설정
        ResolutionUtil.ResetScreenInfo();
        yield return null;
    }
}
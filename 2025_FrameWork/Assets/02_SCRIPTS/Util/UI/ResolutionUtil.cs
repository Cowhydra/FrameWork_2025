using UnityEngine;

public static class ResolutionUtil
{
    private static Vector2 _DeviceScreenSize;   // 실제 디바이스 스크린 사이즈

    private static Vector2Int _ScreenSize;      // Resolution 변경 후 스크린 사이즈

    private static Vector2 _SafeAreaAnchorMin;  // Resolution 변경 후 Safe 영역 Anchor Min 값
    private static Vector2 _SafeAreaAnchorMax;  // Resolution 변경 후 Safe 영역 Anchor Max 값

    private static float _RenderScale;          // 실제 디바이스 스크린 사이즈와 Resolution 변경 후 스크린 사이즈 비율
    private static bool _IsWideScreen;

    // 변경할 기준 해상도
    private const int REF_RESOLUTION_WIDTH = 1280;
    private const int REF_RESOLUTION_HEIGHT = 720;

    // Wide 판단 기준 비율
    private const float REF_WIDE_ASPECT = 16f / 9f; // 1.7777777

    public static Rect DeviceSafeArea { get; private set; } // 실제 디바이스 Safe 영역 사이즈
    public static Rect SafeArea { get; private set; } // Resolution 변경 후 Safe 영역 사이즈

    public static bool IsSafeAreaValid { get; private set; } = false;


    // 디바이스 스크린 사이즈 얻기
    public static void GetDeviceScreenSize()
    {
        int runMode = 0; // 0: Device, 1: Editor, 2: Simulator

#if UNITY_EDITOR
        runMode = (UnityEngine.Device.Application.isMobilePlatform == true ? 2 : 1);
#endif

        if (runMode == 2)
        { 
            _DeviceScreenSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        }
        else
        { 
            _DeviceScreenSize = new Vector2(Screen.width, Screen.height);
        }

        Debug.Log($"DeviceScreenSize : {_DeviceScreenSize}");
    }


    // 디바이스 SafeArea 얻기
    public static void GetDeviceSafeArea()
    {
        DeviceSafeArea = new Rect(Screen.safeArea);

        Debug.Log($"DeviceSafeArea : {DeviceSafeArea}");
    }


    // 기기의 실제 화면 비에 맞춰서 게임 해상도를 720P로 변경
    // 아이패드의 경우엔 가로를 기준으로 1280에 맞춤..
    // 실제 해상도가 기본 해상도보다 작을땐 실제 해상도를 기준으로...
    public static Vector2Int CalcRenderResolution()
    {
        // 기기 화면비
        // 16:9 비율을 기준으로 세로가 길어지는 aspect값은 1.6 이하이다..
        // 5:4 -> 1.25
        // 4:3 -> 1.33..(iPad)
        // 3:2 -> 1.5
        // 16:10 -> 1.6(안드로이드 태블릿)
        // 16:9 -> 1.77..
        // 18.5:9 -> 2.055..(갤S8/S9)
        // 20:9 -> 2.22..(갤S20)
        float aspect = (float)_DeviceScreenSize.x / (float)_DeviceScreenSize.y;

        // 결과값(UNITY해상도의 각축의 값은 int형이다)
        Vector2Int result;

        // 와이드 화면일때
        if (REF_WIDE_ASPECT <= aspect)
        {
            // Height를 고정하고 Width 계산
            int h = Mathf.Min(Screen.height, REF_RESOLUTION_HEIGHT);
            int w = Mathf.RoundToInt(h * aspect);
            result = new Vector2Int(w, h);
        }
        // 아이패드 혹은 서피스 같이 와이드 비율이 아닐때
        else
        {
            // Width를 고정하고 Height 계산
            int w = Mathf.Min(Screen.width, REF_RESOLUTION_WIDTH);
            int h = Mathf.RoundToInt(w / aspect);
            result = new Vector2Int(w, h);
        }

        return result;
    }


    // 해상도 변경
    public static void SetScreenResolution(int width, int height)
    {
        Screen.SetResolution(width, height, true);
    }


    // 스크린 정보 재설정
    public static void ResetScreenInfo()
    {
        _ScreenSize = new Vector2Int(Screen.width, Screen.height);
        _IsWideScreen = ((float)Screen.width / Screen.height >= REF_WIDE_ASPECT);

        // RenderScale 재설정
        ResetRenderScale();

        // SafeArea 재설정
        ResetSafeArea();

        // SafeAreaAnchor 재설정
        ResetSafeAreaAnchor();

        // SafeArea가 있는가? (_ScreenSize와 SafeArea Size가 다르면 SafeArea가 있다는 얘기다.)
        IsSafeAreaValid = ((int)SafeArea.width != _ScreenSize.x || (int)SafeArea.height != _ScreenSize.y);

        Debug.LogFormat("[ResetScreenInfo]\nDevice Resolution : {0}\nTarget Resolution : {1}\nSafe Area : {2}\nIsSafeAreaValid : {3}", _DeviceScreenSize, _ScreenSize, SafeArea, IsSafeAreaValid);
    }


    private static void ResetRenderScale()
    {
        if (_IsWideScreen)
        { 
            _RenderScale = _ScreenSize.y / _DeviceScreenSize.y;
        }
        else
        { 
            _RenderScale = _ScreenSize.x / _DeviceScreenSize.x;
        }
    }


    private static void ResetSafeArea()
    {
        Rect sa = DeviceSafeArea;

        if (_RenderScale != 1.0f)
        { 
            sa = new Rect(Vector2Int.RoundToInt(sa.position * _RenderScale), Vector2Int.RoundToInt(sa.size * _RenderScale));
        }

        // 양쪽을 자르는 경우
        if (sa.width != _ScreenSize.x)
        {
            float r = _ScreenSize.x - sa.x - sa.width;
            float cut = Mathf.Max(sa.x, r);

            sa.x = cut;
            sa.width = _ScreenSize.x - (cut * 2);
        }

        SafeArea = sa;
    }


    private static void ResetSafeAreaAnchor()
    {
        // 일단 SafeArea의 Min과 Max 값으로 초기화 하고
        _SafeAreaAnchorMin = SafeArea.position;
        _SafeAreaAnchorMax = _SafeAreaAnchorMin + SafeArea.size;

        // ScreenSize로 나눠서 (0 ~ 1)사이 값으로 바꾼다.
        _SafeAreaAnchorMin.x = Mathf.Clamp01(_SafeAreaAnchorMin.x / _ScreenSize.x);
        _SafeAreaAnchorMin.y = Mathf.Clamp01(_SafeAreaAnchorMin.y / _ScreenSize.y);
        _SafeAreaAnchorMax.x = Mathf.Clamp01(_SafeAreaAnchorMax.x / _ScreenSize.x);
        _SafeAreaAnchorMax.y = Mathf.Clamp01(_SafeAreaAnchorMax.y / _ScreenSize.y);
    }


    public static void ApplySafeArea(RectTransform rt)
    {
        if (rt == null || IsSafeAreaValid == false)
        { 
            return;
        }

        // rt 영역이 SafeAreaAnchor 영역 내에서 어느 정도 차지하는 지
        float minX = Mathf.Lerp(_SafeAreaAnchorMin.x, _SafeAreaAnchorMax.x, rt.anchorMin.x);
        float minY = Mathf.Lerp(_SafeAreaAnchorMin.y, _SafeAreaAnchorMax.y, rt.anchorMin.y);
        float maxX = Mathf.Lerp(_SafeAreaAnchorMin.x, _SafeAreaAnchorMax.x, rt.anchorMax.x);
        float maxY = Mathf.Lerp(_SafeAreaAnchorMin.y, _SafeAreaAnchorMax.y, rt.anchorMax.y);

        rt.anchorMin = new Vector2(minX, minY);
        rt.anchorMax = new Vector2(maxX, maxY);
    }
}
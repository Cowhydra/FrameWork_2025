using UnityEngine;

public static class ResolutionUtil
{
    private static Vector2 _DeviceScreenSize;   // ���� ����̽� ��ũ�� ������

    private static Vector2Int _ScreenSize;      // Resolution ���� �� ��ũ�� ������

    private static Vector2 _SafeAreaAnchorMin;  // Resolution ���� �� Safe ���� Anchor Min ��
    private static Vector2 _SafeAreaAnchorMax;  // Resolution ���� �� Safe ���� Anchor Max ��

    private static float _RenderScale;          // ���� ����̽� ��ũ�� ������� Resolution ���� �� ��ũ�� ������ ����
    private static bool _IsWideScreen;

    // ������ ���� �ػ�
    private const int REF_RESOLUTION_WIDTH = 1280;
    private const int REF_RESOLUTION_HEIGHT = 720;

    // Wide �Ǵ� ���� ����
    private const float REF_WIDE_ASPECT = 16f / 9f; // 1.7777777

    public static Rect DeviceSafeArea { get; private set; } // ���� ����̽� Safe ���� ������
    public static Rect SafeArea { get; private set; } // Resolution ���� �� Safe ���� ������

    public static bool IsSafeAreaValid { get; private set; } = false;


    // ����̽� ��ũ�� ������ ���
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


    // ����̽� SafeArea ���
    public static void GetDeviceSafeArea()
    {
        DeviceSafeArea = new Rect(Screen.safeArea);

        Debug.Log($"DeviceSafeArea : {DeviceSafeArea}");
    }


    // ����� ���� ȭ�� �� ���缭 ���� �ػ󵵸� 720P�� ����
    // �����е��� ��쿣 ���θ� �������� 1280�� ����..
    // ���� �ػ󵵰� �⺻ �ػ󵵺��� ������ ���� �ػ󵵸� ��������...
    public static Vector2Int CalcRenderResolution()
    {
        // ��� ȭ���
        // 16:9 ������ �������� ���ΰ� ������� aspect���� 1.6 �����̴�..
        // 5:4 -> 1.25
        // 4:3 -> 1.33..(iPad)
        // 3:2 -> 1.5
        // 16:10 -> 1.6(�ȵ���̵� �º�)
        // 16:9 -> 1.77..
        // 18.5:9 -> 2.055..(��S8/S9)
        // 20:9 -> 2.22..(��S20)
        float aspect = (float)_DeviceScreenSize.x / (float)_DeviceScreenSize.y;

        // �����(UNITY�ػ��� ������ ���� int���̴�)
        Vector2Int result;

        // ���̵� ȭ���϶�
        if (REF_WIDE_ASPECT <= aspect)
        {
            // Height�� �����ϰ� Width ���
            int h = Mathf.Min(Screen.height, REF_RESOLUTION_HEIGHT);
            int w = Mathf.RoundToInt(h * aspect);
            result = new Vector2Int(w, h);
        }
        // �����е� Ȥ�� ���ǽ� ���� ���̵� ������ �ƴҶ�
        else
        {
            // Width�� �����ϰ� Height ���
            int w = Mathf.Min(Screen.width, REF_RESOLUTION_WIDTH);
            int h = Mathf.RoundToInt(w / aspect);
            result = new Vector2Int(w, h);
        }

        return result;
    }


    // �ػ� ����
    public static void SetScreenResolution(int width, int height)
    {
        Screen.SetResolution(width, height, true);
    }


    // ��ũ�� ���� �缳��
    public static void ResetScreenInfo()
    {
        _ScreenSize = new Vector2Int(Screen.width, Screen.height);
        _IsWideScreen = ((float)Screen.width / Screen.height >= REF_WIDE_ASPECT);

        // RenderScale �缳��
        ResetRenderScale();

        // SafeArea �缳��
        ResetSafeArea();

        // SafeAreaAnchor �缳��
        ResetSafeAreaAnchor();

        // SafeArea�� �ִ°�? (_ScreenSize�� SafeArea Size�� �ٸ��� SafeArea�� �ִٴ� ����.)
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

        // ������ �ڸ��� ���
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
        // �ϴ� SafeArea�� Min�� Max ������ �ʱ�ȭ �ϰ�
        _SafeAreaAnchorMin = SafeArea.position;
        _SafeAreaAnchorMax = _SafeAreaAnchorMin + SafeArea.size;

        // ScreenSize�� ������ (0 ~ 1)���� ������ �ٲ۴�.
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

        // rt ������ SafeAreaAnchor ���� ������ ��� ���� �����ϴ� ��
        float minX = Mathf.Lerp(_SafeAreaAnchorMin.x, _SafeAreaAnchorMax.x, rt.anchorMin.x);
        float minY = Mathf.Lerp(_SafeAreaAnchorMin.y, _SafeAreaAnchorMax.y, rt.anchorMin.y);
        float maxX = Mathf.Lerp(_SafeAreaAnchorMin.x, _SafeAreaAnchorMax.x, rt.anchorMax.x);
        float maxY = Mathf.Lerp(_SafeAreaAnchorMin.y, _SafeAreaAnchorMax.y, rt.anchorMax.y);

        rt.anchorMin = new Vector2(minX, minY);
        rt.anchorMax = new Vector2(maxX, maxY);
    }
}
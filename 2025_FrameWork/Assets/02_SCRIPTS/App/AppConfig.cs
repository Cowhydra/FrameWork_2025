
public static class AppConfig
{
    // 이 App 버전
    public static int APP_VERSION = 1;

    // 현재 라이브 중인 App 버전 
    public static int LIVE_APP_VER = 0;

    // 번들 버전 (기본적으로 AppVer.minor)
    public static float BUNDLE_VER = 0f;

    // 검수 중인가?
    // 현재 App 버전(APP_VERSION)이 CDN에 저장되어 있는 App 버전(LIVE_APP_VER) 보다 큰 경우
    // iOS : 검수 버전, 검수 서버로 접속
    // Android : 개발 서버로 접속 (패치 전 테스트)
    public static bool IsUnderInspection { get; set; } = false;


    // 번들 버전 체크
    // 게임 중에 존 이동 시 번들 체크 (번들을 새로 올렸는데 유저가 안 받는 경우 대비)
    public static bool CheckIfBundleVersionIsValid(float bundleVer)
    {

        // 현재 번들 버전과 CDN에 저장된 번들 버전과 비교
        return (BUNDLE_VER >= bundleVer);
    }
}
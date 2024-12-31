using System;

public static class TimeUtil
{
    public static bool HasElapsed(DateTime startTime, int durationInSeconds)
    {
        // 현재 시간을 가져옵니다.
        DateTime currentTime = DateTime.Now;

        // 기준 시간에 duration을 더한 값을 계산합니다.
        DateTime targetTime = startTime.AddSeconds(durationInSeconds);

        // 현재 시간이 기준 시간보다 같거나 크면 true를 반환합니다.
        return currentTime >= targetTime;
    }
}

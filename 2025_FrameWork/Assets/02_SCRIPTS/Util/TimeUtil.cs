using System;

public static class TimeUtil
{
    public static bool HasElapsed(DateTime startTime, int durationInSeconds)
    {
        // ���� �ð��� �����ɴϴ�.
        DateTime currentTime = DateTime.Now;

        // ���� �ð��� duration�� ���� ���� ����մϴ�.
        DateTime targetTime = startTime.AddSeconds(durationInSeconds);

        // ���� �ð��� ���� �ð����� ���ų� ũ�� true�� ��ȯ�մϴ�.
        return currentTime >= targetTime;
    }
}

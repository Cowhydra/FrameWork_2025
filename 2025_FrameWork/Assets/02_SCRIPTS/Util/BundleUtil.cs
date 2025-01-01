using D_F_Enum;
using UnityEngine;

public static class BundleUtil 
{
    public static bool IsNetworkValid()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    /// <summary> ���� Cache �� <paramref name="requiredSize"/> �̻��� ���� ������ �ִ°�? </summary>
    public static bool IsDiskSpaceEnough(long requiredSize)
    {
        return Caching.defaultCache.spaceFree >= requiredSize;
    }

    public static long OneGB = 1000000000;
    public static long OneMB = 1000000;
    public static long OneKB = 1000;

    /// <summary> ����Ʈ <paramref name="byteSize"/> ����� �°Բ� ������ ���� <see cref="SizeUnits"/> Ÿ���� �����´� </summary>
    public static E_SIZE GetProperByteUnit(long byteSize)
    {
        if (byteSize >= OneGB)
            return E_SIZE.GB;
        else if (byteSize >= OneMB)
            return E_SIZE.MB;
        else if (byteSize >= OneKB)
            return E_SIZE.KB;
        return E_SIZE.Byte;
    }

    /// <summary> ����Ʈ�� <paramref name="byteSize"/> <paramref name="unit"/> ������ �°� ���ڸ� ��ȯ�Ѵ� </summary>
    public static long ConvertByteByUnit(long byteSize, E_SIZE unit)
    {
        return (long)((byteSize / (double)System.Math.Pow(1024, (long)unit)));
    }

    /// <summary> ����Ʈ�� <paramref name="byteSize"/> ������ �Բ� ����� ������ ���ڿ� ���·� ��ȯ�Ѵ� </summary>
    public static string GetConvertedByteString(long byteSize, E_SIZE unit, bool appendUnit = true)
    {
        string unitStr = appendUnit ? unit.ToString() : string.Empty;
        return $"{ConvertByteByUnit(byteSize, unit).ToString("0.00")}{unitStr}";
    }



    //������ ����
    public static string GetMonsterAssetLabel(int modelNumber)
    {
        return $"model/monster/Enemy_{modelNumber:D3}";
    }
}

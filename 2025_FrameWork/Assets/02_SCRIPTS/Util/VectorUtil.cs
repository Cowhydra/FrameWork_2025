using UnityEngine;

public static  class VectorUtil 
{
    public static Vector3 GetRandomPositionAroundPlayer(Vector3 center, float radius, float offsetY = 5f)
    {
        float angle = Random.Range(0, 360);
        float distance = Random.Range(0, radius);

        float x = center.x + Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        float z = center.z + Mathf.Sin(angle * Mathf.Deg2Rad) * distance;


        //경사진 맵이 있으면 offsetY  을 통해서 위의 길을 찾아야함

        return new Vector3(x, center.y, z);
    }
}

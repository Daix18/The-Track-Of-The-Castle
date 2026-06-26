using UnityEngine;

public class CarouselLayout
{
    public static Vector3 GetPositionForIndex(int index, int totalItems, float radius)
    {
        float angle = (360f / totalItems) * index;
        float radian = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radian) * radius;
        float z = Mathf.Sin(radian) * radius;
        return new Vector3(x, 0, z);
    }

    public static float GetDynamicRadius(int totalItems, float spacing, float maxRadius)
    {
        if (totalItems <= 1)
        {
            return spacing; 
        }

        float radius = spacing / (2f * Mathf.Sin(Mathf.PI / totalItems));
        return Mathf.Min(radius, maxRadius);
    }
}



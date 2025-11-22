using UnityEngine;

public class Utils
{
    public static bool IsCloseTo(Vector3 source, Vector3 target)
    {
        return IsInRange(source, target, 0.0f, 1.0f);
    }

    public static bool IsInRange(Vector3 source, Vector3 target, float rangeMax)
    {
        return IsInRange(source, target, 0.0f, rangeMax);
    }

    public static bool IsInRange(Vector3 source, Vector3 target, float rangeMin, float rangeMax)
    {
        Vector3 a = source;
        Vector3 b = target;

        a.y = 0.0f;
        b.y = 0.0f;

        float magnitude = (b - a).magnitude;

        return rangeMin <= magnitude && magnitude <= rangeMax;
    }
}

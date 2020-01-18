using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomLib
{
    public static Vector3 MoveInDirection(Vector3 current, Vector3 target, float distance)
    {
        float angle = Mathf.Atan2(target.y - current.y, target.x - current.x);
        return MoveInDirection(current, angle, distance);
    }
    public static Vector3 MoveInDirection(Vector3 current, float angle, float distance)
    {
        return new Vector3(current.x + Mathf.Cos(angle) * distance, current.y + Mathf.Sin(angle) * distance, 0f);
    }

    public static float AngleFromPos(Vector2 origin, Vector2 target) // Returns an angle between 0 and 360 degrees
    {
        float rad = Mathf.Atan2(target.y - origin.y, target.x - origin.x);
        float deg = Mathf.Rad2Deg * rad;
        if (deg < 0f)
            deg += 360f;
        return deg;
    }
}

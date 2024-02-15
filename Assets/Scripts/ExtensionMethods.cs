using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 Rotate(this Vector2 v, float angle)
    {
        float r = v.magnitude;
        float a = v.RotationAngleRad();
        angle *= Mathf.Deg2Rad;
        return r * new Vector2(Mathf.Cos(a + angle), Mathf.Sin(a + angle));
    }

    public static float RotationAngleDeg(this Vector2 v)
    {
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public static float RotationAngleRad(this Vector2 v)
    {
        return Mathf.Atan2(v.y, v.x);
    }

    public static Vector2 ToVector2XY(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 ToVector2XZ(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector2 ToVector2ZY(this Vector3 v)
    {
        return new Vector2(v.z, v.y);
    }

    public static Vector3 ToVector3XY(this Vector2 v)
    {
        return new Vector3(v.x, v.y, 0f);
    }

    public static Vector3 ToVector3XZ(this Vector2 v)
    {
        return new Vector3(v.x, 0f, v.y);
    }

    public static Vector3 ToVector3ZY(this Vector2 v)
    {
        return new Vector3(0f, v.y, v.x);
    }

    public static Vector3 ZeroY(this Vector3 v)
    {
        return new Vector3(v.x, 0f, v.z);
    }
}

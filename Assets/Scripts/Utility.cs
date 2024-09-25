using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class Utility : IUtility
{
    public Vector2 RandomPointInCircle(Vector2 center, float radius)
    {
        // 生成[-1, 1]范围内的随机值
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);

        // 将随机值缩放到圆内
        Vector2 point = new Vector2(randomX, randomY).normalized * radius;

        // 将点移到圆心
        point += center;

        return point;
    }

    public Vector3 RotateVector(Vector3 position, Vector3 center, Vector3 axis, float angle)
    {
        return Quaternion.AngleAxis(angle, axis) * (position - center) + center;
    }
}

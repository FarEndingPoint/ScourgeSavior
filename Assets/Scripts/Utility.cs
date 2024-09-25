using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class Utility : IUtility
{
    public Vector2 RandomPointInCircle(Vector2 center, float radius)
    {
        // ����[-1, 1]��Χ�ڵ����ֵ
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);

        // �����ֵ���ŵ�Բ��
        Vector2 point = new Vector2(randomX, randomY).normalized * radius;

        // �����Ƶ�Բ��
        point += center;

        return point;
    }

    public Vector3 RotateVector(Vector3 position, Vector3 center, Vector3 axis, float angle)
    {
        return Quaternion.AngleAxis(angle, axis) * (position - center) + center;
    }
}

using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletForwardMove : EnemyBullet //������ǰֱ��ǰ��
{
    [SerializeField] float speed;

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
    }
}

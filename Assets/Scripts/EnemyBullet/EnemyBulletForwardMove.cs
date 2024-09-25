using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletForwardMove : EnemyBullet //持续向前直线前进
{
    [SerializeField] float speed;

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletUpCast : EnemyBullet
{
    [SerializeField] float force;

    private void Start()
    {
        rb.AddForce(transform.right * force, ForceMode2D.Impulse);
    }
}

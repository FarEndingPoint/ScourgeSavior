using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletTrack : EnemyBullet
{
    [SerializeField] float speed;

    private void FixedUpdate()
    {
        rb.velocity = (PlayerController.Instance.transform.position - transform.position).normalized * speed;
    }
}

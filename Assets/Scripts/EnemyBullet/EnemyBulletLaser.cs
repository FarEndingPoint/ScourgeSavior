using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletLaser : EnemyBullet
{
    [SerializeField] float existTime;

    private void Update()
    {
        existTime -= Time.deltaTime;
        if(existTime <= 0) Destroy(gameObject);
    }

    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            TypeEventSystem.Global.Send<PlayerBeHitedEvent>();
    }
}

using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Destroy(gameObject);
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            TypeEventSystem.Global.Send<PlayerBeHitedEvent>();
            Destroy(gameObject);
        }
    }
}

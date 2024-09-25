using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletFixedTimeToDispear : EnemyBullet
{
    [SerializeField] float existTime;
    [SerializeField] float startForce;
    [SerializeField] float forwardForce;
    [SerializeField] float sideForce;
    float t;

    private void Start()
    {
        rb.AddForce(Vector2.up * startForce);
        t= Random.Range(0.0f, 1.0f); ;
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.right * forwardForce);
        if (t <= 0.5f) rb.AddForce(-transform.up * sideForce);
        else rb.AddForce(transform.up * sideForce);
        existTime -= Time.fixedDeltaTime;
        if(existTime <= 0) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            TypeEventSystem.Global.Send<PlayerBeHitedEvent>();
            Destroy(gameObject);
        }
    }
}

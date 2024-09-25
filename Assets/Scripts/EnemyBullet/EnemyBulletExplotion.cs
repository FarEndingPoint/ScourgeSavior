using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletExplotion : EnemyBullet
{
    [SerializeField] float speed;
    [SerializeField] float beforeExplotionTime;
    float curBeforeExplotionTime;
    Animator animator;
    new BoxCollider2D collider;

    private void Start()
    {
        curBeforeExplotionTime = beforeExplotionTime;
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (curBeforeExplotionTime > 0)
        {
            rb.velocity = transform.right * speed;
        }
        else rb.velocity =Vector2.zero;
    }

    private void Update()
    {
        if(curBeforeExplotionTime > 0)
        {
            curBeforeExplotionTime -= Time.deltaTime;
        }
        else
        {
            animator.SetTrigger("Explotion");
            collider.size = new Vector2(0.8f, 0.8f);
            collider.offset = new Vector2(-0.08f, 0);
        }
    }

    public void ExplotionEnd()
    {
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (curBeforeExplotionTime > 0)
            {
                curBeforeExplotionTime = 0;
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (curBeforeExplotionTime > 0)
            {
                curBeforeExplotionTime = 0;
            }
            TypeEventSystem.Global.Send<PlayerBeHitedEvent>();
        }

    }
}

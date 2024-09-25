using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullerTwistingForwardMove : EnemyBullet
{
    [SerializeField] float speed;

    private void Start()
    {
        StartCoroutine(Twisting());
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
    }

    IEnumerator Twisting()
    {
        while(true) 
        {
            transform.DOMoveY(0.1f, 0.4f);
            yield return new WaitForSeconds(0.4f);
            transform.DOMoveY(-0.1f, 0.4f);
            yield return new WaitForSeconds(0.4f);
        }
    }
}

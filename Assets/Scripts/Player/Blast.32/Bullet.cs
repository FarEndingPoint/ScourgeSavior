using QFramework;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;
    PlayerAttackToAddSlotEvent addEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        addEvent.shootDelta = 0;
        addEvent.furyDelta = 0.01f;
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            TypeEventSystem.Global.Send(addEvent);
            collision.transform.GetComponent<Enemy>().BeHited(Enemy.BeHitedType.Shoot);
            if (collision.transform.TryGetComponent<Xeno>(out Xeno xeno))
                xeno.BeShooted(false, transform.Position2D());
            Destroy(gameObject);
        }
    }
}

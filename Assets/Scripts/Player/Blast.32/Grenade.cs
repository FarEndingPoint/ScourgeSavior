using UnityEngine;
using QFramework;

public class Grenade : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] GameObject explotion;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Instantiate(explotion, transform.Position2D(), Quaternion.identity);
            AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Explotion");
            Destroy(gameObject);
        }
    }
}

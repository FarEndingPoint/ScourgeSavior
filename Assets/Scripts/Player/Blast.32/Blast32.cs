using QFramework;
using UnityEngine;

public class Blast32 : MonoBehaviour
{
    Vector3 leftScale = new Vector3(-3.2f, 3.2f, 1);
    Vector3 rightScale = new Vector3(3.2f, 3.2f, 1);
    [SerializeField] Transform playerPosition;
    [SerializeField] Transform followPosition;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject grenade;
    float shootTime = 0.2f;
    float curShootTime = 0;
    Vector3 target;

    private void Start()
    {
        transform.position = followPosition.Position2D();
    }

    void Update()
    {
        transform.position = Vector2.Lerp(transform.Position2D(), followPosition.Position2D(), 0.05f);
        Rotate(playerPosition.Position2D().x >= transform.Position2D().x);
    }

    public void Shoot(bool isChargeShoot, Vector2 target)
    {
        GameObject go;
        if (isChargeShoot) go = Instantiate(grenade, transform.position, Quaternion.identity);
        else go = Instantiate(bullet, transform.position, Quaternion.identity);
        go.transform.right = (target - go.transform.Position2D()).normalized;
        this.target = target;
        curShootTime = shootTime;
    }

    void Rotate(bool isRight)
    {
        transform.localScale = isRight ? rightScale : leftScale;
        if (curShootTime > 0)
        {
            curShootTime -= Time.deltaTime;
            if(isRight) transform.right = Vector2.Lerp(transform.right, target - transform.position, 0.05f);
            else transform.right = Vector2.Lerp(transform.right, transform.position - target, 0.05f);
        }
        else transform.right = Vector2.right;
    }
}

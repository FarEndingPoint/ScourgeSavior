using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class BloodFly : Xeno
{
    [SerializeField] Transform _attackPoint;
    [SerializeField] float foundRadius;

    XenoCircleDetectPlayerCommand xenoCircleDetectPlayerCommand;

    protected override void Awake()
    {
        base.Awake();
        xenoCircleDetectPlayerCommand = new XenoCircleDetectPlayerCommand(attackPoint, xenoModel.meetTimeToFoundPlayer, foundRadius);
    }

    protected override bool DetectPlayer()
    {
        return this.SendCommand(xenoCircleDetectPlayerCommand);
    }

    protected override void Move()
    {
        if (!aiPath.canMove) aiPath.canMove = true;
        else transform.localScale = aidDestinationSetter.target.position.x >= transform.position.x ? xenoModel.rightScale : xenoModel.leftScale;
    }

    protected override void Stop()
    {
        aiPath.canMove = false;
    }

    public void SelfExplosion()
    {
        Destroy(gameObject);
        LevelController.Instance.KillEnemy();
        Instantiate(bullet, attackPoint.position, Quaternion.identity).transform.right = Vector2.up;
        Instantiate(bullet, attackPoint.position, Quaternion.identity).transform.right = Vector2.down;
        Instantiate(bullet, attackPoint.position, Quaternion.identity).transform.right = Vector2.left;
        Instantiate(bullet, attackPoint.position, Quaternion.identity).transform.right = Vector2.right;
        Instantiate(bullet, attackPoint.position, Quaternion.identity).transform.right = new Vector2(1, 1);
        Instantiate(bullet, attackPoint.position, Quaternion.identity).transform.right = new Vector2(1, -1);
        Instantiate(bullet, attackPoint.position, Quaternion.identity).transform.right = new Vector2(-1, 1);
        Instantiate(bullet, attackPoint.position, Quaternion.identity).transform.right = new Vector2(-1, -1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPoint.position, foundRadius);
    }
}

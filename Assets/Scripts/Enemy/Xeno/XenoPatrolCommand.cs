using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XenoPatrolCommand : AbstractCommand
{
    PatrolMoveXeno patrolMoveXeno;
    Transform transform;
    XenoModel xenoModel;

    public XenoPatrolCommand(PatrolMoveXeno patrolMoveXeno, Transform transform, XenoModel xenoModel)
    {
        this.patrolMoveXeno = patrolMoveXeno;
        this.transform = transform;
        this.xenoModel = xenoModel;
        patrolMoveXeno.patrolCenter = transform.position;
    }

    protected override void OnExecute()
    {
        if (patrolMoveXeno.curPatrolTime > 0)
        {
            if (Vector3.Distance(transform.position, patrolMoveXeno.patrolPoint) > 0.05f)
                transform.Translate((patrolMoveXeno.patrolPoint - transform.position).normalized * patrolMoveXeno.patrolSpeed * Time.deltaTime);
            patrolMoveXeno.curPatrolTime -= Time.deltaTime;
        }
        else StartPartrol();
    }

    public void StartPartrol()
    {
        patrolMoveXeno.curPatrolTime = patrolMoveXeno.patrolTime;
        patrolMoveXeno.patrolPoint = RandomPointInCircle(patrolMoveXeno.patrolCenter, patrolMoveXeno.patrolRadius);
        if (patrolMoveXeno.patrolPoint.x >= transform.position.x) transform.localScale = xenoModel.rightScale;
        else transform.localScale = xenoModel.leftScale;
    }

    Vector2 RandomPointInCircle(Vector2 center, float radius)
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        Vector2 point = new Vector2(randomX, randomY).normalized * radius;
        point += center;
        return point;
    }
}

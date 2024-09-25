using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XenoCircleDetectPlayerCommand : AbstractCommand<bool>
{
    Transform transform;
    float meetTimeToFoundPlayer, timer, radius;
    bool isMeet;

    public XenoCircleDetectPlayerCommand(Transform transform, float meetTimeToFoundPlayer, float radius)
    {
        this.transform = transform;
        this.meetTimeToFoundPlayer = meetTimeToFoundPlayer;
        this.radius = radius;
        timer = 0;
        isMeet = false;
    }

    protected override bool OnExecute()
    {
        if (Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player")) != null)
        {
            if (isMeet)
            {
                if (timer <= 0)
                {
                    isMeet = false;
                    return true;
                }
                else timer -= Time.deltaTime;
            }
            else
            {
                isMeet = true;
                timer = meetTimeToFoundPlayer;
            }
        }
        else isMeet = false;
        return false;
    }
}

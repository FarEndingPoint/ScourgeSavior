using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class XenoLineDetectPlayerCommand : AbstractCommand<bool>
{
    Transform transform;
    float meetTimeToFoundPlayer, timer;
    bool isMeet;

    public XenoLineDetectPlayerCommand(Transform transform, float meetTimeToFoundPlayer)
    {
        this.transform = transform;
        this.meetTimeToFoundPlayer = meetTimeToFoundPlayer;
        timer = 0;
        isMeet = false;
    }

    protected override bool OnExecute()
    {
        if (Physics2D.Raycast(transform.position, PlayerController.Instance.transform.position - transform.position, Mathf.Infinity, LayerMask.GetMask("Player", "Ground")).collider.gameObject.layer == LayerMask.NameToLayer("Player"))
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class XenoBeShootedCommand : AbstractCommand
{
    Transform transform;
    Rigidbody2D rb;
    XenoModel xenoModel;

    public XenoBeShootedCommand(Transform transform, Rigidbody2D rb, XenoModel xenoModel)
    {
        this.transform = transform;
        this.rb = rb;
        this.xenoModel = xenoModel;
    }

    protected override void OnExecute()
    {
        if (xenoModel.curBeShootedTime > 0)
        {
            xenoModel.curBeShootedTime -= Time.deltaTime;
            rb.AddForce((transform.Position2D() - xenoModel.beShootedPoint).normalized * xenoModel.beShootedSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
        if (xenoModel.curBeChargeShootedTime > 0)
        {
            xenoModel.curBeChargeShootedTime -= Time.deltaTime;
            rb.AddForce((xenoModel.beShootedOriginalPos - xenoModel.beShootedPoint).normalized * xenoModel.beChargeShootedSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
        if (xenoModel.curBeShootedTime <= 0 && xenoModel.curBeChargeShootedTime <= 0)
            xenoModel.canMove = true;
    }
}

using QFramework;
using UnityEngine;

public class GroundDetectorCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<PlayerModel>().isGround = PlayerController.Instance.bottom.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (this.GetModel<PlayerModel>().isGround) this.GetModel<PlayerModel>().canSecondJump = true;
    }
}

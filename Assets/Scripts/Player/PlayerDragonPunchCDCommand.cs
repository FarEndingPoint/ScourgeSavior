using QFramework;
using UnityEngine;

public class PlayerDragonPunchCDCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        if (Main.Interface.GetModel<PlayerModel>().curDragonPunchCD.Value > 0)
        {
            Main.Interface.GetModel<PlayerModel>().curDragonPunchCD.Value -= Time.deltaTime;
            Main.Interface.GetModel<PlayerModel>().canDragonPunch = false;
        }
        else Main.Interface.GetModel<PlayerModel>().canDragonPunch = true;
    }
}

using ns;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public void CountdownEnd()
    {
        UIKit.ClosePanel<ReadyPanel>();
        Main.Interface.GetSystem<InputSystem>().inputController.Enable();
        LevelController.Instance.BeginPlay();
    }
}

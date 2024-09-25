using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class InputSystem : AbstractSystem
{
    public InputController inputController { get; private set; }

    protected override void OnInit()
    {
        inputController = new InputController();
    }
}

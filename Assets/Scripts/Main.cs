using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : Architecture<Main>
{
    protected override void Init()
    {
        RegisterModel(new PlayerModel());
        RegisterSystem(new InputSystem());
        RegisterSystem(new ActionSystem());
        RegisterUtility(new Utility());
        RegisterModel(new GameData());
    }
}

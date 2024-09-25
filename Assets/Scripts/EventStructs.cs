using System;

public struct MoveStateOnUpdateEvent
{
    public string parameterName;
    public Action callback;
}

public struct FightStateOnUpdateEvent
{
    public Action callbackInAction, callbackOutAction;
}

public struct DragonPunchJudgeEvent
{
    public bool isSmash;
}

public struct PlayerBeHitedEvent
{
}

public struct PlayerAttackToAddSlotEvent
{
    public float shootDelta, furyDelta;
}


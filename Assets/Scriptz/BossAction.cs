using UnityEngine;
public enum BossActionType
{
    Attack,
    ShortPause,
    LongPause,
    NoAttack
    // Add more actions as needed
}
public class BossAction
{
    public BossActionType ActionType;

    public BossAction(BossActionType actionType)
    {
        ActionType = actionType;
    }
}

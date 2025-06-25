using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attack")]
public class Attack : ScriptableObject
{
    public string Name;
    public string animTriggerName;

    public List<TurnAction> Actions = new List<TurnAction>();
    
    // public int Damage;
    // public int Overheat;
}
public enum Target
{
    Self,
    Opponent
}
public enum ActionType
{
    Damage,
    Overheat
}

[System.Serializable]
public class TurnAction
{
    public Target Target;
    public ActionType Type;
    public int Amount;
}
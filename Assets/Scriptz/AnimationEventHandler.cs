using System;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    private CharacterTurnBased _characterTurnBased;


    private void Awake()
    {
        _characterTurnBased = GetComponentInParent<CharacterTurnBased>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void PerformAttack()
    {
        _characterTurnBased.PerformAction();
    }

    public void AttackEnded()
    {
        _characterTurnBased.AttackEnded();
    }
}

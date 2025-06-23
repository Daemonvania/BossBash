using System;
using System.Collections.Generic;
using UnityEngine;
public class BossStateManager : MonoBehaviour
{
    //how to have the code be the same for both preparation and execution phases, only changing the end move
    public float smallPauseDuration = 0.5f;
    public float longPauseDuration = 1.0f;

    public GameObject flashObject;
    public GameObject laserBeam;
    
    BossBaseState currentState;
    [HideInInspector] public BossAttackState attackState = new BossAttackState(); 
    [HideInInspector] public BossFlashState flashState = new BossFlashState(); 
    
    [HideInInspector] public List<BossAction> bossActions; 
    
    private BossActionGenerator actionGenerator;

    public event Action onAttack;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actionGenerator = GetComponent<BossActionGenerator>();
        flashObject.SetActive(false);
        laserBeam.SetActive(false);
        
        StartAttackSequence();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    
    public void StartAttackSequence()
    {
        bossActions = actionGenerator.GenerateBossActionList();
        SwitchState(flashState);
    }
    public void SwitchState(BossBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    public void BroadcastAttack()
    {
        onAttack?.Invoke();
    }
}

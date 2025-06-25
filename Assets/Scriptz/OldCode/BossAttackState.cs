using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BossAttackState: BossBaseState
{

    public override void EnterState(BossStateManager stateManager)
    {
        ExecuteBossActions(stateManager.bossActions, stateManager);
    }

    public override void UpdateState(BossStateManager stateManager)
    {
        
    }
    
    private async void ExecuteBossActions(List<BossAction> actions, BossStateManager stateManager)
    {
        if (Application.isPlaying != true) { return; };
        foreach (var action in actions)
        {
            switch (action.ActionType)
            {
                case BossActionType.Attack:
                    Attack(stateManager); 
                    break;
                case BossActionType.ShortPause:
                    await Task.Delay(Mathf.RoundToInt(stateManager.smallPauseDuration * 1000));
                    break;
                case BossActionType.LongPause:
                    await Task.Delay(Mathf.RoundToInt(stateManager.longPauseDuration * 1000));
                    break;
                case BossActionType.NoAttack:
                   
                    break;
            }
        }
        await Task.Delay(2000);
        stateManager.StartAttackSequence();
    }
    
    private async void Attack(BossStateManager stateManager)
    {
        // Activate the laser beam
        stateManager.laserBeam.SetActive(true);
        await Task.Delay(50);
        stateManager.BroadcastAttack();
        // Wait for a short duration to simulate the attack
        await Task.Delay(150);
        
        // Deactivate the laser beam
        stateManager.laserBeam.SetActive(false);
    }
}

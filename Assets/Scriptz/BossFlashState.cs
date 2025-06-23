using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BossFlashState: BossBaseState
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
                    Flash(stateManager); 
                    break;
                case BossActionType.ShortPause:
                    await Task.Delay(Mathf.RoundToInt(stateManager.smallPauseDuration * 1000));
                    break;
                case BossActionType.LongPause:
                    await Task.Delay(Mathf.RoundToInt(stateManager.longPauseDuration * 1000));
                    break;
            }
        }

        await Task.Delay(1000);
        stateManager.SwitchState(stateManager.attackState);
    }
    
    private async void Flash(BossStateManager stateManager)
    {
        // Activate the flash object
        stateManager.flashObject.SetActive(true);
        
        // Wait for a short duration
        await Task.Delay(150);
        
        // Deactivate the flash object
        stateManager.flashObject.SetActive(false);
    }
}

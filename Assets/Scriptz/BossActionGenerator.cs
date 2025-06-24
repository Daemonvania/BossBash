using System.Collections.Generic;
using UnityEngine;

public class BossActionGenerator : MonoBehaviour
{
    
    [HideInInspector] public int attackNumber = 3;
    
    public List<BossAction> GenerateBossActionList()
    {
        // Ensure the game is running to use Random.Range correctly
        List<BossAction> bossActions = new List<BossAction>();
        
        for (int i = 0; i < attackNumber; i++)
        {
            // Add an attack action
            bossActions.Add(new BossAction(BossActionType.Attack));
            bossActions.Add(new BossAction(
            Random.Range(0, 2) == 0 ? BossActionType.ShortPause : BossActionType.LongPause
            ));
            
            // bossActions.Add(new BossAction(
            //     Random.Range(0, 2) == 0 ? BossActionType.Attack: BossActionType.NoAttack
            // ));
            // bossActions.Add(new BossAction(BossActionType.ShortPause));
        }
        
        return bossActions;
    }
}

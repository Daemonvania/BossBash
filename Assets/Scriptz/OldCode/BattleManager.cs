using System;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private BossStateManager bossStateManager;


    private void OnEnable()
    {
        // Subscribe to the attack event from the boss state manager
        bossStateManager.onAttack += player.OnBossAttack;
    }
    
    private void OnDisable()
    {
        // Unsubscribe from the attack event to prevent memory leaks
        bossStateManager.onAttack -= player.OnBossAttack;
    }
}

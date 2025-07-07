using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private Attack[] BossAttacks;

    public Attack ChooseBossAttack(CharacterTurnBased characterTurnBased)
    {
        Attack BossAttack;
        if (characterTurnBased.GetOverheatPercent() <= 0.25)
        {
            BossAttack = BossAttacks[Random.Range(0, BossAttacks.Length - 1)];
        }
        else
        {
            BossAttack = BossAttacks[Random.Range(0, BossAttacks.Length)];
        }
        
        return BossAttack;
    }

}

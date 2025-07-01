using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private Attack[] BossAttacks;

    public Attack ChooseBossAttack()
    {
        Attack BossAttack = BossAttacks[Random.Range(0, BossAttacks.Length)];
        return BossAttack;
    }

}

using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private Attack BossAttack;

    public Attack ChooseBossAttack()
    {
        return BossAttack;
    }

}

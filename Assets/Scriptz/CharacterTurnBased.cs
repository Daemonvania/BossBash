using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTurnBased : MonoBehaviour
{
     public bool isPlayer;
    
    private Animator animator;
    private Attack currentAttack = null;
    
    public event Action<int, ActionType> OnAttacked;
    public event Action OnAttackEnded;
    private HealthSystem healthSystem;
    private OverheatSystem overheatSystem;
    private HealthBarManager _healthBarManager;

   [HideInInspector] public int overheatTicker = 0;
   [HideInInspector] public bool isOverheating { get; private set; }

    private void Awake()
    {
        //can move these to Initializefight on the BattleHandlerTurns
        animator = GetComponentInChildren<Animator>();
        _healthBarManager = GetComponent<HealthBarManager>();
        healthSystem = new HealthSystem(100);
        overheatSystem = new OverheatSystem(100);
        healthSystem.OnHealthChanged += OnHealthChanged;
        overheatSystem.OnOverheatChanged += OnHealthChanged;
        isOverheating = false;
    }

    private void Start()
    {
        OnHealthChanged();
    }

    private enum State {
        Idle,
        Busy,
    }


    public void TurnStarted()
    {
        if (isPlayer)
        {
            animator.SetBool("Defend", false);
        }
    }
    public void OpponentAttacking()
    {
        if (isPlayer)
        {
            animator.SetBool("Defend", true);
        }
    }
    public void Damage(int damageAmount)
    {
        if (isOverheating)
        {
            damageAmount = Mathf.RoundToInt(damageAmount * 1.5f);
        }
        else if (damageAmount > 0)
        {
            animator.SetTrigger("Hurt");
        }

        healthSystem.Damage(damageAmount);

        if (healthSystem.IsDead()) {
            // Died
        }
    }
    public void Overheat(int overheatAmount) {
        overheatSystem.AddOverheat(overheatAmount);

        if (overheatSystem.IsOverheating())
        {
            Debug.Log("Overheating");
            isOverheating = true;
            animator.SetBool("Overheat", true);
        }
    }

    public void EndOverheat()
    {
        isOverheating = false;
        overheatTicker = 0;
        animator.SetBool("Overheat", false);
        overheatSystem.SetOverheatAmount(0);
    }

    public bool IsDead() {
        return healthSystem.IsDead();
    }

    public void StartAction(Attack attack)
    {
        Debug.Log(attack.Name);
        currentAttack = attack;
        animator.SetBool(attack.animTriggerName, true);
    }

    //called from animation event
    public void PerformAction()
    {
        //could add number of hits, then divide the attack damage by number of hits , then should be able to go to overheat animation with no exit time and stop teh attackEnded func

        foreach (var action in currentAttack.Actions)
        {
            switch (action.Target)
            {
                case Target.Self:
                    switch (action.Type)
                    {
                        case ActionType.Damage:
                            Damage(action.Amount);
                            break;
                        case ActionType.Overheat:
                            Overheat(action.Amount);
                            break;
                    }
                    break;

                case Target.Opponent:
                    OnAttacked?.Invoke(action.Amount, action.Type);
                    break;
            }
        }
    }
    public void AttackEnded()
    {
        animator.SetBool(currentAttack.animTriggerName, false);
        OnAttackEnded?.Invoke();

        currentAttack = null;
    }

    void OnHealthChanged()
    {
        _healthBarManager.UpdateBarsGradual(healthSystem.GetHealthPercent(), overheatSystem.GetOverheatPercent());
    }
    
}

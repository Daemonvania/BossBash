using System;
using System.Collections;
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
   [SerializeField] int maxHealth = 100;
   [SerializeField] int maxOverheat = 100;

   [Space] [SerializeField] private GameObject weakAttackParticle;
   [SerializeField] private GameObject strongAttackParticle;
   [SerializeField] private GameObject overheatParticle;
   [SerializeField] private GameObject cooldownParticle;
   [SerializeField] private GameObject overheatOpponentParticle;
   [SerializeField] private GameObject healparticle;
   [Space]
   [SerializeField] private AudioClip weakAttackSound;
   [SerializeField] private AudioClip strongAttackSound;
   [SerializeField] private AudioClip overheatSound;
   [SerializeField] private AudioClip cooldownSound;
   [SerializeField] private AudioClip overheatOpponentSound;
   [SerializeField] private AudioClip healSound;
   [SerializeField] private AudioClip hurtSound;
   
   
   
   [HideInInspector] public bool isOverheating { get; private set; }

    private void Awake()
    {
        //can move these to Initializefight on the BattleHandlerTurns
        animator = GetComponentInChildren<Animator>();
        _healthBarManager = GetComponent<HealthBarManager>();
        healthSystem = new HealthSystem(maxHealth);
        overheatSystem = new OverheatSystem(maxOverheat);
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
            SoundManager.instance.PlaySoundClip(hurtSound, transform, 0.45f);
        }

        healthSystem.Damage(damageAmount);

        if (healthSystem.IsDead()) {
            EndOverheat();
            animator.SetBool("Dead", true);
        }
    }
    public void Overheat(int overheatAmount) {
        overheatSystem.AddOverheat(overheatAmount);

        if (overheatSystem.IsOverheating())
        {
            Debug.Log("Overheating");
            isOverheating = true;
            animator.SetBool("Overheat", true);
            overheatParticle.SetActive(true);
            SoundManager.instance.PlaySoundClip(overheatSound, transform, 0.6f);

        }
    }

    public void EndOverheat()
    {
        isOverheating = false;
        overheatTicker = 0;
        animator.SetBool("Overheat", false);
        overheatSystem.SetOverheatAmount(0);
        overheatParticle.SetActive(false);
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
    
    
    public void PlayParticle(string particleName)
    {
        if (particleName == "WeakAttack")
        {
            Debug.Log("WeakAttack");
            weakAttackParticle.SetActive(true);
            StartCoroutine(SetParticleInactive(weakAttackParticle));
            SoundManager.instance.PlaySoundClip(weakAttackSound, transform, 0.45f);
        }
        if (particleName == "StrongAttack")
        {
            strongAttackParticle.SetActive(true);
            StartCoroutine(SetParticleInactive(strongAttackParticle));
            SoundManager.instance.PlaySoundClip(strongAttackSound, transform, 0.45f);
        }
        if (particleName == "Heal")
        {
            healparticle.SetActive(true);
            StartCoroutine(SetParticleInactive(healparticle));
            SoundManager.instance.PlaySoundClip(healSound, transform, 0.6f);

        }
        if (particleName == "Cooldown")
        {
            cooldownParticle.SetActive(true);
            StartCoroutine(SetParticleInactive(cooldownParticle));
            SoundManager.instance.PlaySoundClip(cooldownSound, transform, 0.6f);

        }
        if (particleName == "OverheatOpponent")
        {
            overheatOpponentParticle.SetActive(true);
            StartCoroutine(SetParticleInactive(overheatOpponentParticle));
            SoundManager.instance.PlaySoundClip(overheatOpponentSound, transform, 0.7f);

        }
    }

    private IEnumerator SetParticleInactive(GameObject gameObject)
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
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

    public float GetHealthPercent()
    {
        return healthSystem.GetHealthPercent();
    }
    public float GetOverheatPercent()
    {
        return overheatSystem.GetOverheatAmount();
    }
    
}

using System;
using System.Collections;
using UnityEngine;

public class BattleHandlerTurns : MonoBehaviour
{
    enum BattleState
    {
        Start,
        Playerturn,
        EnemyTurn,
        Busy,
        WaitingForPlayer,
        End
    }

    private BattleState currentState;
    
    private static BattleHandlerTurns instance;

    public static BattleHandlerTurns GetInstance() {
        return instance;
    }


    // [SerializeField] private Transform pfCharacterBattle;

    [SerializeField] CharacterTurnBased playerCharacterBattle;
    [SerializeField] CharacterTurnBased enemyCharacterBattle;

    [SerializeField] private int _overheatFalloffPerTurn = 5;
    private CharacterTurnBased activeCharacterBattle;

    private BossAI _bossAI;


    public event Action OnPlayerTurn;
    public event Action OnPlayerChoseAction;
    
    private void Awake() {
        instance = this;
        _bossAI = enemyCharacterBattle.gameObject.GetComponent<BossAI>();
    }
    
    private void Start() {

        SetActiveCharacterBattle(playerCharacterBattle);
        currentState = BattleState.WaitingForPlayer;
    }

    private void OnEnable()
    {
        playerCharacterBattle.OnAttacked += OnAttacked;
        enemyCharacterBattle.OnAttacked += OnAttacked;
        
        playerCharacterBattle.OnAttackEnded += OnAttackEnded;
        enemyCharacterBattle.OnAttackEnded += OnAttackEnded;
    }

    //Called from UI or Enemy AI
    public void Attack(Attack attack)
    {
        if (currentState == BattleState.Busy) {return;}
        currentState = BattleState.Busy;
        if (activeCharacterBattle == playerCharacterBattle)
        {
            Debug.Log("StartingPlayerAttack");
            OnPlayerChoseAction?.Invoke();
            playerCharacterBattle.StartAction(attack);
        }
        if (activeCharacterBattle == enemyCharacterBattle)
        {
            Debug.Log("StartingBossAttack");
            enemyCharacterBattle.StartAction(attack);
        }
    }

    void OnAttacked(int damage, ActionType actionType)
    {
        if (activeCharacterBattle == playerCharacterBattle)
        {
            PerformAttack(enemyCharacterBattle, damage, actionType );
        }
        else if (activeCharacterBattle == enemyCharacterBattle)
        {
            PerformAttack(playerCharacterBattle, damage, actionType );
        }
    }

    void PerformAttack(CharacterTurnBased target ,int amount, ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Damage:
                target.Damage(amount);
                break;
            case ActionType.Overheat:
                target.Overheat(amount);
                break;
        }
    }
    
    void OnAttackEnded()
    {
        ChooseNextActiveCharacter();    
    }
    
    private void ChooseNextActiveCharacter()
    {
        if (TestBattleOver())
            return;

        bool isPlayerTurn = activeCharacterBattle == playerCharacterBattle;

        CharacterTurnBased current = isPlayerTurn ? enemyCharacterBattle : playerCharacterBattle;
        CharacterTurnBased previous = isPlayerTurn ? playerCharacterBattle : enemyCharacterBattle;

        SetActiveCharacterBattle(current);

        HandleOverheatTicker(previous);
        if (HandleOverheatSkip(current))
            return;

        if (isPlayerTurn)
        {
            Debug.Log("EnemyAICalled");
            currentState = BattleState.EnemyTurn;
            Attack(_bossAI.ChooseBossAttack());
        }
        else
        {
            StartPlayerTurn();
            currentState = BattleState.WaitingForPlayer;
        }
    }
    private void HandleOverheatTicker(CharacterTurnBased character)
    {
        if (!character.isOverheating) return;

        character.overheatTicker++;
        if (character.overheatTicker > 2)
        {
            character.EndOverheat();
            Debug.Log($"EndOverheat{(character == playerCharacterBattle ? "Player" : "Boss")}");
        }
    }

    private bool HandleOverheatSkip(CharacterTurnBased character)
    {
        if (!character.isOverheating) return false;

        if (character.overheatTicker <= 2)
        {
            // TODO: Optional: show "OVERHEATED" popup
            StartCoroutine(SkipTurn(character));
            return true;
        }

        // Already handled in HandleOverheatTicker if ticker > 2
        return false;
    }



    IEnumerator SkipTurn(CharacterTurnBased character)
    {
        character.overheatTicker++;
        yield return new WaitForSeconds(1);
        ChooseNextActiveCharacter();
    }

    void StartPlayerTurn()
    {
        OnPlayerTurn?.Invoke();
        playerCharacterBattle.Overheat(-5);
        enemyCharacterBattle.Overheat(-5);
    }
    
    private void SetActiveCharacterBattle(CharacterTurnBased characterBattle) {
        if (activeCharacterBattle != null) {
   
        }

        activeCharacterBattle = characterBattle;
    }
    
    private bool TestBattleOver() {
        if (playerCharacterBattle.IsDead()) {
            // Player dead, enemy wins
            //CodeMonkey.CMDebug.TextPopupMouse("Enemy Wins!");
         
            return true;
        }
        if (enemyCharacterBattle.IsDead()) {
            // Enemy dead, player wins
            //CodeMonkey.CMDebug.TextPopupMouse("Player Wins!");
       
            return true;
        }

        return false;
    }
}

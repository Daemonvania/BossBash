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
    
    private void ChooseNextActiveCharacter() {
        if (TestBattleOver()) {
            return;
        }
        if (activeCharacterBattle == playerCharacterBattle) {
            SetActiveCharacterBattle(enemyCharacterBattle);
            //if opponent overheating add to the ticker
            if (playerCharacterBattle.isOverheating)
            {
                playerCharacterBattle.overheatTicker++;
                if (playerCharacterBattle.overheatTicker > 2)
                {
                    playerCharacterBattle.EndOverheat();
                    Debug.Log("EndOverheatBoss");
                }
            }
            if (enemyCharacterBattle.isOverheating)
            {
                //if overheated last turn skip turn
                if (enemyCharacterBattle.overheatTicker <= 2)
                {
                    StartCoroutine(SkipTurn(enemyCharacterBattle));
                    return;
                }
                //if overheated before remove overheat
                if (enemyCharacterBattle.overheatTicker > 2)
                {
                    enemyCharacterBattle.EndOverheat();
                    Debug.Log("EndOverheatBoss");
                }
            }
            //perform boss attack based on AI
            currentState = BattleState.EnemyTurn;
           Attack(_bossAI.ChooseBossAttack());
        } else {
            SetActiveCharacterBattle(playerCharacterBattle);
            //if opponent overheating add to the ticker
            if (enemyCharacterBattle.isOverheating)
            {
                enemyCharacterBattle.overheatTicker++;
                if (enemyCharacterBattle.overheatTicker > 2)
                {
                    enemyCharacterBattle.EndOverheat();
                    Debug.Log("EndOverheatBoss");
                }
            }
            //if overheated skip turn
            if (playerCharacterBattle.isOverheating)
            {
                //if overheated last turn skip turn
                if (playerCharacterBattle.overheatTicker <= 2)
                {
                    //todo can Have a lil "OVERHEATED" popup here.
                    StartCoroutine(SkipTurn(playerCharacterBattle));
                    return;
                }
                //if overheated before remove overheat
                if (playerCharacterBattle.overheatTicker > 2)
                {
                    playerCharacterBattle.EndOverheat();
                    Debug.Log("EndOverheatPlayer");
                }
            }
            //Show Player UI & other 
            StartPlayerTurn();
            currentState = BattleState.WaitingForPlayer;
        }
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

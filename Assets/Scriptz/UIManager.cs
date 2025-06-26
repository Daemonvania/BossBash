using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerUI;
    [SerializeField] private GameObject MainButtons;
    [SerializeField] private GameObject AttackButtons;
    [SerializeField] private GameObject DefendButtons;

    [SerializeField] private MenuEventSystemHandler _menuEventSystemHandler;
    
    private BattleHandlerTurns _battleHandlerTurns;

    private void Awake()
    {
        _battleHandlerTurns = GetComponent<BattleHandlerTurns>();
        ShowMainMenu();
    }

    private void OnEnable()
    {
        _battleHandlerTurns.OnPlayerTurn += OnPlayerTurn;
        _battleHandlerTurns.OnPlayerChoseAction += OnPlayerChoseAction;
    }
    private void OnDisable()
    {
        _battleHandlerTurns.OnPlayerTurn -= OnPlayerTurn;
        _battleHandlerTurns.OnPlayerChoseAction -= OnPlayerChoseAction;
    }
    private void OnPlayerTurn()
    {
        TogglePlayerUI(true);
        ShowMainMenu();
    }
    
    private void OnPlayerChoseAction()
    {
        TogglePlayerUI(false);
    }
    
    public void TogglePlayerUI(bool enabled)
    {
        PlayerUI.SetActive(enabled);
    }
    
    
    public void ShowAttackButtons()
    {
        _menuEventSystemHandler.ResetAllScalesAnimated();
        MainButtons.SetActive(false);
        AttackButtons.SetActive(true);
    }
    
    public void ShowDefendButtons()
    {
        _menuEventSystemHandler.ResetAllScalesAnimated();
        MainButtons.SetActive(false);
        DefendButtons.SetActive(true);
    }

    public void ShowMainMenu()
    {
        _menuEventSystemHandler.ResetAllScalesAnimated();
        AttackButtons.SetActive(false);
        DefendButtons.SetActive(false);
        MainButtons.SetActive(true);
    }
}

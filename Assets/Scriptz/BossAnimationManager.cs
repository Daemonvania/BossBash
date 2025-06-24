using System;
using Unity.VisualScripting;
using UnityEngine;

public class BossAnimationManager : MonoBehaviour
{
    private BossStateManager _stateManager;

    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _stateManager = GetComponent<BossStateManager>();
    }

    private void OnEnable()
    {
        _stateManager.OnEnteredState += OnEnteredState;
    }
    private void OnDisable()
    {
        _stateManager.OnEnteredState -= OnEnteredState;
    }

    void OnEnteredState(BossBaseState state)
    {
        Debug.Log("ObserverState works");
        switch (state)
        {
            case BossFlashState:
                _animator.SetBool("Shooting", false);
                break;
            case BossAttackState:
                Debug.Log("EnterShootin");
                _animator.SetBool("Shooting", true);
                break;
        }
    }
}

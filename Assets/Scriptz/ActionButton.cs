using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ActionButton : MonoBehaviour
{
    [Header("Action To Perform")] [SerializeField]
    private Attack action;
    
    [SerializeField] private GameObject info;
    
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text infoText;

    private Vector3 _originalPosition;
    
    //todo Perhaps will do the attack call here instead of in button, then it has access to all the info and the button stuff is annoying
    private void Start()
    {
        info.SetActive(false);
        // _originalPosition = transform.position;
    }

    private void OnEnable()
    {
        // transform.position = _originalPosition + new Vector3(-100f, 0f, 0f); // Adjust -100f to how far left you want

        // Animate to the original position
        // transform.DOMove(_originalPosition, 0.5f).SetEase(Ease.OutCubic);

        name.text = action.Name;
        infoText.text = action.Description;
    }

    public void OnClicked()
    {
        BattleHandlerTurns.GetInstance().Attack(action);
    }

    private void OnDisable()
    {
        
    }

    public void ShowInfo()
    {
        info.SetActive(true);
    }
    
    public void HideInfo()
    {
        info.SetActive(false);
    }
}

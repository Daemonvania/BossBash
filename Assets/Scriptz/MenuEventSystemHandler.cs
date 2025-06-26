using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class MenuEventSystemHandler : MonoBehaviour
{
    [Header("References")] 
    public List<Selectable> Selectables = new List<Selectable>();

    [Header("Animations")] 
    [SerializeField]
    protected float _selectedAnimationScale = 1.1f;

    [SerializeField] protected float _scaleDuration = 0.25f;

    protected Dictionary<Selectable, Vector3> _scales = new Dictionary<Selectable, Vector3>();

    protected Tween _scaleUpTween;
    protected Tween _scaleDownTween;

    public virtual void Awake()
    {
        foreach (var selectable in Selectables)
        {
            AddSelectionListeners(selectable);
            _scales.Add(selectable, selectable.transform.localScale);
        }
    }

    public virtual void OnEnable()
    {
        for (int i = 0; i < Selectables.Count; i++)
        {
            Selectables[i].transform.localScale = _scales[Selectables[i]];
        }
    }

    public virtual void OnDisable()
    {
        _scaleUpTween.Kill(true);
        _scaleDownTween.Kill(true);
    }

    protected virtual void AddSelectionListeners(Selectable selectable)
    {
        EventTrigger trigger = selectable.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = selectable.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry SelectEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Select
        };
        SelectEntry.callback.AddListener(OnSelect);
        trigger.triggers.Add(SelectEntry);
        
        EventTrigger.Entry DeselectEntry = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.Deselect
        };
        DeselectEntry.callback.AddListener(OnDeselect);
        trigger.triggers.Add(DeselectEntry);

        EventTrigger.Entry PointerEnter = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerEnter
        };
        PointerEnter.callback.AddListener(OnPointerEnter);
        trigger.triggers.Add(PointerEnter);

        EventTrigger.Entry PointerExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        PointerExit.callback.AddListener(OnPointerExit);
        trigger.triggers.Add(PointerExit);

    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selecteeed");
        Vector3 newScale = eventData.selectedObject.transform.localScale * _selectedAnimationScale;
        _scaleUpTween = eventData.selectedObject.transform.DOScale(newScale, _scaleDuration);

        ActionButton actionButton = eventData.selectedObject.GetComponent<ActionButton>();
        if (actionButton != null)
        {
            actionButton.ShowInfo(); //todo interface instead so that I can have different functionality on each script "OnSelect"
        }

    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        Selectable sel = eventData.selectedObject.GetComponent<Selectable>();
        _scaleDownTween = eventData.selectedObject.transform.DOScale(_scales[sel], _scaleDuration);
        
        ActionButton actionButton = eventData.selectedObject.GetComponent<ActionButton>();
        if (actionButton != null)
        {
            actionButton.HideInfo(); 
        }
    }

    public void OnPointerEnter(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (pointerEventData != null)
        {
            pointerEventData.selectedObject = pointerEventData.pointerEnter;
        }
    }

    public void OnPointerExit(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (pointerEventData != null)
        {
            pointerEventData.selectedObject = null;
        }
    }
    
}

using System;
using DG.Tweening;
using UnityEngine;

public class ButtonLayoutGroup : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector2 _originalAnchoredPosition;
    private CanvasGroup _canvasGroup;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalAnchoredPosition = _rectTransform.anchoredPosition;
        _canvasGroup = GetComponent<CanvasGroup>();

    }

    private void OnEnable()
    {
        // Move it to the left first
        _rectTransform.anchoredPosition = _originalAnchoredPosition + new Vector2(-100f, 0f); // Adjust -100f as needed

        // Animate to the original position
        _rectTransform.DOAnchorPos(_originalAnchoredPosition, 0.5f).SetEase(Ease.OutCubic);
        _canvasGroup.alpha = 0f;
        _canvasGroup.DOFade(1f, 0.35f);
    }
    
}

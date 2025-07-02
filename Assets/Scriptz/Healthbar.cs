using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public class Healthbar : MonoBehaviour
{

    [SerializeField] private Image  _healthBarFillImage;
    [SerializeField] private Image _healthBarTrailing;
    [SerializeField] private float _trailDelay = 0.4f;

    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DrainHealthBar()
    {
        float ratio = _currentHealth / _maxHealth;
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_healthBarFillImage.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        sequence.AppendInterval(_trailDelay);
        sequence.Append(_healthBarTrailing.DOFillAmount(ratio, 0.3f)).SetEase(Ease.OutSine);
        sequence.Play();
    }
}

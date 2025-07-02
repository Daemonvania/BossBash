using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private Image  _healthBarFillImage;
    [SerializeField] private Image _healthBarTrailing;
    
    [SerializeField] private Image  _overheatBarFillImage;
    [SerializeField] private Image _overheatTrailing;
    
    
    
    [SerializeField] private float _trailDelay = 0.4f;


    public void UpdateBarsInstant(float healthFillPercent, float overheatPercent)
    {
        _healthBarFillImage.fillAmount = healthFillPercent;
        _healthBarTrailing.fillAmount = healthFillPercent;
        
        _overheatBarFillImage.fillAmount = overheatPercent;
        _overheatTrailing.fillAmount = overheatPercent;
    } 
    
    public void UpdateBarsGradual(float healthFillPercent, float overheatPercent)
    {
        Debug.Log(healthFillPercent);
        Debug.Log(overheatPercent);
        
        Sequence sequence1 = DOTween.Sequence();

        sequence1.Append(_healthBarFillImage.DOFillAmount(healthFillPercent, 0.25f)).SetEase(Ease.InOutSine);
        sequence1.AppendInterval(_trailDelay);
        sequence1.Append(_healthBarTrailing.DOFillAmount(healthFillPercent, 0.3f)).SetEase(Ease.OutSine);
        sequence1.Play();
        
        Sequence sequence2 = DOTween.Sequence();

        sequence2.Append(_overheatBarFillImage.DOFillAmount(overheatPercent, 0.25f)).SetEase(Ease.InOutSine);
        sequence2.AppendInterval(_trailDelay);
        sequence2.Append(_overheatTrailing.DOFillAmount(overheatPercent, 0.3f)).SetEase(Ease.OutSine);
        sequence2.Play();
    }
    
}

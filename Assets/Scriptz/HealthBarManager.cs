using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider overheatBar;


    public void UpdateBars(float healthFillPercent, float overheatPercent)
    {
        healthBar.value = healthFillPercent;
        overheatBar.value = overheatPercent;
    } 
}

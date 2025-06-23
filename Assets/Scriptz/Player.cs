using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField] float parryLength = 0.25f; // Duration of the parry in seconds
    
    enum PlayerState
    {
        Idle,
        Attacking,
        Parrying
    }
    bool isParrying = false;
    
    PlayerState currentState = PlayerState.Parrying;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAttack(InputValue value)
    {
            if (currentState == PlayerState.Parrying && !isParrying)
            {
                Debug.Log("Parry Attempt");
                isParrying = true;
               StartCoroutine(EndParry());
            }
    }
    
    public void OnBossAttack()
    {
        if (isParrying)
        {
            Debug.Log("parried attack");
        }
        else
        {
            Debug.Log("got hit");
        }
    }
    
    private IEnumerator EndParry()
    {
        yield return new WaitForSeconds(parryLength);
        isParrying = false;
    }
}

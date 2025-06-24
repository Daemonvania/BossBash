using UnityEngine;

public class FloatFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // The target to follow
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, 3f * Time.deltaTime);
    }
}

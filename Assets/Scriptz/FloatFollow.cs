using UnityEngine;

public class FloatFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // The target to follow
    [SerializeField] private float amplitude = 0.5f; // Amplitude of the sine wave
    [SerializeField] private float frequency = 2f; // Frequency of the sine wave

    private float initialY; // Initial Y position of the object

    void Start()
    {
        // Store the initial Y position
        initialY = transform.position.y;
    }

    void Update()
    {
        // Calculate the sine wave offset
        float sineOffset = Mathf.Sin(Time.time * frequency) * amplitude;

        // Follow the target's position with the sine wave offset
       // Vector3 targetPosition = new Vector3(target.position.x, target.position.y + sineOffset, target.position.z);
       // transform.position = Vector3.MoveTowards(transform.position, targetPosition, 20f * Time.deltaTime);
      
    }
}

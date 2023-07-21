using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startPosition;
    public Vector3 moveVector;
    [SerializeField] float period = 7f; // overridden
    float moveFactor;
    // visualise Sine value
    // [SerializeField] [Range(0,1)] float moveFactor;

    void Start()
    {
        startPosition = transform.position;
        Debug.Log(startPosition);
    }

    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period; // keeps increasing
        const float tau = Mathf.PI * 2;
        float rawSine = Mathf.Sin(cycles * tau);

        moveFactor = (rawSine + 1f) / 2f; // normalise range -1/+1
        // Debug.Log(rawSine + ' ' + moveFactor);
        Vector3 offset = moveVector * moveFactor;
        transform.position = startPosition + offset;
    }
}

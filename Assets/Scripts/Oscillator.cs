using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startPosition;
    public Vector3 moveVector;
    [SerializeField] [Range(0,1)] float moveFactor;
    [SerializeField] float period = 7f;

    void Start()
    {
        startPosition = transform.position;
        Debug.Log(startPosition);
    }

    void Update()
    {
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSine = Mathf.Sin(cycles * tau);

        moveFactor = (rawSine + 1f) / 2f;
                Debug.Log(rawSine + ' ' + moveFactor);
        Vector3 offset = moveVector * moveFactor;
        transform.position = startPosition + offset;
    }
}

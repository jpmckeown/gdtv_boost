using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startPosition;
    public Vector3 moveVector;
    [SerializeField] [Range(0,1)] float moveFactor;
    // [SerializeField] float period = 2f;

    void Start()
    {
        startPosition = transform.position;
        Debug.Log(startPosition);
    }

    void Update()
    {
        Vector3 offset = moveVector * moveFactor;
        transform.position = startPosition + offset;
    }
}

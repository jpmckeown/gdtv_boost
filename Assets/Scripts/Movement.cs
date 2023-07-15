using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    public float mainThrust = 1000;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ProcessInput();
    }

    void ProcessInput(){
        if(Input.GetKey(KeyCode.Space)) {
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.A)){
            Debug.Log("Left arrow key pressed");
        }
        else if(Input.GetKey(KeyCode.D)){
            Debug.Log("Right arrow key pressed");
        }
    }

}

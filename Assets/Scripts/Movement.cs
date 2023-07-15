using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
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
            rb.AddRelativeForce(0,1,0);
            Debug.Log("Space key pressed for rocket motor.");
        }
        if(Input.GetKey(KeyCode.A)){
            Debug.Log("Left arrow key pressed");
        }
        else if(Input.GetKey(KeyCode.D)){
            Debug.Log("Right arrow key pressed");
        }
    }

}

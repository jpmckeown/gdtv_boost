using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        ProcessInput();
    }

    void ProcessInput(){
        if(Input.GetKey(KeyCode.Space)) {
            Debug.Log("Space key pressed for rocket motor.");
        }
        if(Input.GetKey(KeyCode.A)){
            Debug.Log("Left arrow key pressed");
        }
        if(Input.GetKey(KeyCode.D)){
            Debug.Log("Right arrow key pressed");
        }
    }

}

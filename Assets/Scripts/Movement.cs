using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    AudioSource rocketMainAudio;

    public float mainThrust = 400f;
    public float sideThrust = 80f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rocketMainAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        mainEngine();
        rotateBurst();
    }

    void mainEngine(){
        if(Input.GetKey(KeyCode.Space)) {
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

            if(!rocketMainAudio.isPlaying){
                rocketMainAudio.Play();
            }
        }
        else{
            rocketMainAudio.Stop();
        }
    }

    void rotateBurst(){

        if(Input.GetKey(KeyCode.A))
        {
            ApplyRotation(sideThrust);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-sideThrust);
        }
    }

    void ApplyRotation(float rotationDuringFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationDuringFrame);
        rb.freezeRotation = false;
    }
}

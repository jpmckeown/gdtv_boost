using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float mainThrust = 400f;
    public float sideThrust = 80f;
    public AudioClip mainThrustSound;

    Rigidbody rb;
    AudioSource audioSource;
    // AudioSource rocketMainAudio; // maybe return to this approach instead of .Play...()

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        // rocketMainAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.L)) {
            // LoadNextLevel();
        }
        mainEngine();
        rotateBurst();
    }

    void mainEngine(){
        if(Input.GetKey(KeyCode.Space)) {
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

            audioSource.PlayOneShot(mainThrustSound);
            // if(!rocketMainAudio.isPlaying){
            //     rocketMainAudio.Play();
            // }
        }
        else{
            // mainThrustSound.Stop();
            // rocketMainAudio.Stop();
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

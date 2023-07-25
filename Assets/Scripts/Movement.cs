using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float mainThrust = 160f;
    public float sideThrust = 80f;
    public AudioClip mainThrustSound;

    public ParticleSystem mainDriveParticles;
    public ParticleSystem leftThrustParticles;
    public ParticleSystem rightThrustParticles;

    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        mainEngine();
        rocketRotation();
    }

    void mainEngine(){
        if(Input.GetKey(KeyCode.Space))
        {
            StartMainThrust();
        }
        else
        {
            StopMainThrust();
        }
    }

    void rocketRotation(){

        if(Input.GetKey(KeyCode.A))
        {
            StartLeftThrust();
        }
        else if(Input.GetKey(KeyCode.D))
        {
            StartRightThrust();
        }
        else
        {
            StopSideThrust();
        }
    }

    void StopMainThrust()
    {
        audioSource.Stop();
        mainDriveParticles.Stop();
    }

    void StartMainThrust()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainThrustSound);
        }
        if (!mainDriveParticles.isPlaying)
        {
            mainDriveParticles.Play();
        }
    }

    void StopSideThrust()
    {
        leftThrustParticles.Stop();
        rightThrustParticles.Stop();
    }

    void StartRightThrust()
    {
        ApplyRotation(sideThrust);
        if (!rightThrustParticles.isPlaying)
        {
            rightThrustParticles.Play();
        }
    }

    void StartLeftThrust()
    {
        ApplyRotation(-sideThrust);
        if (!leftThrustParticles.isPlaying)
        {
            leftThrustParticles.Play();
        }
    }

    void ApplyRotation(float rotationDuringFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationDuringFrame);
        rb.freezeRotation = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustForce = 1000f;
    [SerializeField] float rotationTrustForce = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem leftThruster;
    [SerializeField] ParticleSystem rightThruster;
    [SerializeField] ParticleSystem mainThruster;

    Rigidbody myRigidbody;
    AudioSource myAudioSource;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }    

    private void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space)) 
        {
            myRigidbody.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
            if(!myAudioSource.isPlaying)
            {
                myAudioSource.PlayOneShot(mainEngine);
            } 
            if(!mainThruster.isEmitting)
            {
                mainThruster.Play();
            }           
        }        
        else
        {
            myAudioSource.Stop();
            
            StopThruster();
        }
    }

    public void StopThruster()
    {
        mainThruster.Stop();
    }

    private void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationTrustForce);
            if(!rightThruster.isEmitting)
            {
                rightThruster.Play();
            }
        }
        else if(Input.GetKey(KeyCode.D)) 
        {
            ApplyRotation(-rotationTrustForce);
            if(!leftThruster.isEmitting)
            {
                leftThruster.Play();
            }
        }
        else
        {           
            StopSideThrusters();
        }
    }

    public void StopSideThrusters()
    {
        rightThruster.Stop();
        leftThruster.Stop();
    }

    private void ApplyRotation(float roationThisFrame)
    {
        myRigidbody.freezeRotation = true;
        transform.Rotate(Vector3.back * roationThisFrame * Time.deltaTime);
        myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | 
                                  RigidbodyConstraints.FreezeRotationY | 
                                  RigidbodyConstraints.FreezePositionZ;
    }
}

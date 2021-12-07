using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody myRigidbody;
    AudioSource myAudioSource;
    [SerializeField] float thrustForce = 1000f;
    [SerializeField] float rotationTrustForce = 100f;

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
                myAudioSource.Play();
            }            
        }        
        else
        {
            myAudioSource.Stop();
        }
    }
    
    private void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationTrustForce);
        }
        else if(Input.GetKey(KeyCode.D)) 
        {
            ApplyRotation(-rotationTrustForce);
        }
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

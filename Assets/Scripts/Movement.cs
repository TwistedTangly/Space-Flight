using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] GameObject spotLight;
    [SerializeField] GameObject pointLight;
    [SerializeField] float thrustForce = 1000f;
    [SerializeField] float rotationTrustForce = 100f;
    [SerializeField] AudioClip mainEngineAudio;
    [SerializeField] AudioClip sideEngineAudio;
    [SerializeField] ParticleSystem leftThruster;
    [SerializeField] ParticleSystem rightThruster;
    [SerializeField] ParticleSystem mainThruster;

    Rigidbody myRigidbody;
    AudioSource[] myAudioSource;
    AudioSource audioSourceA;
    AudioSource audioSourceB;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponents<AudioSource>();
        audioSourceA = myAudioSource[0];
        audioSourceB = myAudioSource[1];
        pointLight.active = false;
        spotLight.active = false;
    }

    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }    

    private void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {        
            StopThruster();
        }
    }

    void StartThrusting()
    {
        myRigidbody.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
        PlayThrusterAudio();
        if (!mainThruster.isEmitting)
        {
            mainThruster.Play();
        }
        pointLight.active = true;
        spotLight.active = true;
    }

    private void PlayThrusterAudio()
    {
        if (!audioSourceA.isPlaying)
        {
            audioSourceA.PlayOneShot(mainEngineAudio);
        }
    }

    public void StopThruster()
    {
        audioSourceA.Stop();    
        mainThruster.Stop();
        pointLight.active = false;
        spotLight.active = false;
    }

    void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if(Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {           
            StopSideThrusters();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationTrustForce);
        PlaySideThrusterAudio();
        if (!leftThruster.isEmitting)
        {
            leftThruster.Play();
        }
    }

    private void PlaySideThrusterAudio()
    {
        if (!audioSourceB.isPlaying)
        {
            audioSourceB.PlayOneShot(sideEngineAudio, 0.5f);
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationTrustForce);
        PlaySideThrusterAudio();
        if (!rightThruster.isEmitting)
        {
            rightThruster.Play();
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


    public void StopSideThrusters()
    {
        audioSourceB.Stop();
        rightThruster.Stop();
        leftThruster.Stop();
    }
}

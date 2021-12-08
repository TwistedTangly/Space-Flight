using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    Rigidbody myRigidbody;
    Movement movement;
    [SerializeField] float levelLoadDelaySccess = 3f;
    [SerializeField] float levelLoadDelayFail = 1f;
    [SerializeField] AudioClip CrashAudio;
    [SerializeField] AudioClip SuccessAudio;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] GameObject lThruster;
    [SerializeField] GameObject rThruster;

    AudioSource myAudioSource;

    bool isTransistioning = false;
    private void Start() 
    {
        myAudioSource = GetComponent<AudioSource>();
        myRigidbody = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
    }
    private void OnCollisionEnter(Collision other) 
    {
        if(isTransistioning){return;}
        
        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Fuel":
                Debug.Log("Fuel");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default: 
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        movement.StopSideThrusters();
        movement.StopThruster();
        isTransistioning = true;
        myAudioSource.Stop(); 
        movement.enabled = false;        
        GetComponent<MeshRenderer>().enabled = false;
        lThruster.active = false;
        rThruster.active = false;
        myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        explosionParticles.Play();
        myAudioSource.PlayOneShot(CrashAudio);
        Invoke("ReloadLevel", levelLoadDelayFail);  
    }

    void StartSuccessSequence()
    {
        movement.StopSideThrusters();
        movement.StopThruster();
        isTransistioning = true;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(SuccessAudio);;
        movement.enabled = false; 
        Invoke("LoadNextLevel", levelLoadDelaySccess);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex= 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);  
    }
}

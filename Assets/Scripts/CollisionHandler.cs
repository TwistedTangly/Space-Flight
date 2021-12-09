using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    Rigidbody myRigidbody;
    Movement movement;
    BoxCollider myBoxCollider;
    MeshRenderer myMeshRenderer;
    CapsuleCollider myCapsuleCollider;
    [SerializeField] float levelLoadDelaySccess = 3f;
    [SerializeField] float levelLoadDelayFail = 1f;
    [SerializeField] AudioClip CrashAudio;
    [SerializeField] AudioClip SuccessAudio;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] GameObject lThruster;
    [SerializeField] GameObject rThruster;

    AudioSource myAudioSource;

    bool isTransistioning = false;
    bool collisonDisabled = false;
    private void Start() 
    {
        myAudioSource = GetComponent<AudioSource>();
        myRigidbody = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        myBoxCollider = GetComponent<BoxCollider>();
        myCapsuleCollider = GetComponent<CapsuleCollider>();
        myMeshRenderer = GetComponent<MeshRenderer>();
    }
    private void Update() 
    {
        LoadNextLevelOnKeyPress();
        DisableCollision();
    }

    private void DisableCollision()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            collisonDisabled = !collisonDisabled; //toggle from one state to the other - true to false - false to true
            Debug.Log("Collion changed");
            if(collisonDisabled)
            {
                myMeshRenderer.materials[2].color = Color.white;
            }
            else
            {
                myMeshRenderer.materials[2].color = Color.red;
            }
            //completelydisable the collisons to pass throught objects
            //myCapsuleCollider.enabled = false;
            //myBoxCollider.enabled = false;
        }
    }

    private void LoadNextLevelOnKeyPress()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(isTransistioning || collisonDisabled){return;}
        
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

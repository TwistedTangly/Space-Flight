using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip CrashAudio;
    [SerializeField] AudioClip SuccessAudio;

    AudioSource myAudioSource;

    bool isTransistioning = false;
    private void Start() 
    {
        myAudioSource = GetComponent<AudioSource>();
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
        isTransistioning = true;
        myAudioSource.Stop();
         GetComponent<Movement>().enabled = false;
        myAudioSource.PlayOneShot(CrashAudio);
        Invoke("ReloadLevel", levelLoadDelay);  
    }

    void StartSuccessSequence()
    {
        isTransistioning = true;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(SuccessAudio);
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
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

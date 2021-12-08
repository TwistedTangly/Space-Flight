using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingCollisionHandler : MonoBehaviour
{
    [SerializeField] ParticleSystem SuccessParticles;
            
    private void OnCollisionEnter(Collision other) 
    {
        SuccessParticles.Play();
    }
}

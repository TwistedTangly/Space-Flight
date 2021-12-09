using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    
    [SerializeField] float period = 2f;
    float movementFactor;
    void Start()
    {
        startingPosition = transform.position;
    }


    void Update()
    {
        if(period <= Mathf.Epsilon){return;}                //mathf.Epsilon is a incredibly small floating point value 
        float cycles = Time.time / period;                  //continually growing
        const float tau = Mathf.PI *2;                      //constant value of 6.283
        float rawSinWave = Mathf.Sin(cycles * tau);         // between -1 and 1
        movementFactor = (rawSinWave + 1f) / 2f;            //between 0 and 1
        Vector3 offset = movementVector * movementFactor;   //betwwen 0 and set value so counts up and down from 0 to set value
        transform.position = startingPosition + offset;     //between the startingPosition and what the offset is set as every frame
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveinDirection : MonoBehaviour
{
    [SerializeField] float movementFactorX;
    [SerializeField] float movementFactorY;
    [SerializeField] float movementFactorZ;
    Rigidbody myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        myRigidbody.velocity = new Vector3(movementFactorX, movementFactorY, movementFactorZ);
    }
}

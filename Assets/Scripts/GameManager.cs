using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //trackable variables
    public bool hasShooter;
    public bool hasPickup;
    public Vector3 leftHandDefPos;
    public Vector3 rightHandDefPos;

    public Quaternion defHandRotation = Quaternion.Euler(0f,0f,0f);

    [SerializeField] GameObject rightHand;
    [SerializeField] GameObject leftHand;
    void Start()
    {
        //setting default state of variables
        hasPickup = false;
        hasShooter = false;

        leftHandDefPos = leftHand.transform.position;
        rightHandDefPos = rightHand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

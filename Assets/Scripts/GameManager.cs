using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //trackable variables
    public bool hasShooter;
    public bool hasPickup;
    // Start is called before the first frame update
    void Start()
    {
        //setting default state of variables
        hasPickup = false;
        hasShooter = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

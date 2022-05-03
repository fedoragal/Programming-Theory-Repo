using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{

    //variables for the gameobjects involved
    private GameObject pickup;
    private GameObject leftHand;
    private GameObject rightHand;
    private GameObject spawned;
    private GameObject otherObj;
    private GameObject thisObj;

    [SerializeField] private GameManager gameManager;

    [SerializeField] public GameObject shooter;

    //variables to store defaults
    private Vector3 shooterOffset = new Vector3(.003f, 0.113f, 0.022f);
    private Quaternion shooterRotation = Quaternion.Euler(7.153f, -79.569f, 0.854f);

    private void Start() {
        //grab the gamemanager script
        thisObj = GetComponent<GameObject>();
    }

    private void OnCollisionEnter(Collision other) {
        //assign the gameobject we hit to a variable
        otherObj = other.gameObject;

        //checks to see which object this is and what collided with it
        if(CompareTag("shooter") && otherObj.CompareTag("player")){
            PickupShooter(thisObj);
           // Debug.Log("I hit a Shooter");
        }

        if(CompareTag("powerup") && otherObj.CompareTag("player")){
            PickupPowerUp(thisObj);
            // Debug.Log("I hit a powerUp");
        }
    }

    private void PickupShooter(GameObject pickup){
        //grab left hand and right hand gameobjects
        foreach(Transform child in otherObj.GetComponentInChildren<Transform>()){
            if(child.CompareTag("RightHand")){
                rightHand = child.gameObject;
                child.transform.position = gameManager.rightHandDefPos;
                child.transform.rotation = gameManager.defHandRotation;
            } else if(child.CompareTag("LeftHand")){
                leftHand = child.gameObject;
                child.transform.position = gameManager.leftHandDefPos;
                child.transform.rotation = gameManager.defHandRotation;
            }
        }

        //check to see if the player already has a shooter
        if(gameManager.hasShooter){
            //get rid of the existing shooter
        } else {
            // set the hasShooter variable true
            gameManager.hasShooter = true;
        }

        //spawn the new shooter
            Vector3 shooterLocation = rightHand.transform.position;
            spawned = Instantiate(shooter, shooterLocation, shooterRotation);
            spawned.transform.parent = rightHand.transform;
            spawned.transform.position += shooterOffset;

        //destroy the pickup object on the ground
            Destroy(gameObject);
    }

    private void PickupPowerUp(GameObject pickup){

    }
}

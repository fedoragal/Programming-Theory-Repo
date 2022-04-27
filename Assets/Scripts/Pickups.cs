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
    private Vector3 shooterOffset = new Vector3(-0.245f, 0.32f, 0.981f);
    private Quaternion shooterRotation = Quaternion.Euler(24.302f, -60.383f, 5.516f);

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

        //check to see if the player already has a shooter
        if(gameManager.hasShooter){
            //get rid of the existing shooter
        } else {
            // set the hasShooter variable true
            gameManager.hasShooter = true;
        }

        //spawn the new shooter
            Vector3 shooterLocation = otherObj.transform.position;
            spawned = Instantiate(shooter, shooterLocation, shooterRotation);
            spawned.transform.parent = otherObj.transform;
            spawned.transform.position += shooterOffset;

        //destroy the pickup object on the ground
            Destroy(thisObj.gameObject);
    }

    private void PickupPowerUp(GameObject pickup){

    }
}

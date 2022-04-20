using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //setting up the basic game to keyboard as movement & mouselook
    public KeyCode forward = KeyCode.W;
    public KeyCode back = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode jump = KeyCode.Space;

    //movement speeds
    public float lookSpeed = 3.0f;
    public float forwardSpeed = 5.0f;
    public float strafeSpeed = 5.0f;
    public float jumpForce = 500.0f;

    //camera variables
    private Camera playerCamera;

    //rotation variables for camera and player
    public float minCamAngleX = -45; //minimum angle the camera can look ie. down
    public float maxCamAngleX = 80; //maximum angle the camera can look ie. up
    Vector2 rotation = Vector2.zero; //vector for storing mouse inputs
    private float rotY = 0.0f; //rotate around the right/y axis
    private float rotX = 0.0f; //rotate around the up/x axis

    //physics variables
    private Rigidbody playerRB;

    // Start is called before the first frame update
    void Start()
    {
        //assign player camera to variable
        playerCamera = GetComponentInChildren<Camera>();
        playerRB = GetComponent<Rigidbody>();

        //grab the current rotation of the character - just cause
        Vector2 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

    }

    // Update is called once per frame
    void Update()
    {

        #region Basic Movement

        if(Input.GetKey(forward)){
            //move forward
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
        }

        if(Input.GetKey(back)){
            //move forward
            transform.Translate(Vector3.forward * -forwardSpeed * Time.deltaTime);
        }

        if(Input.GetKey(right)){
            //move forward
            transform.Translate(Vector3.right * forwardSpeed * Time.deltaTime);
        }

        if(Input.GetKey(left)){
            //move forward
            transform.Translate(Vector3.right * -forwardSpeed * Time.deltaTime);
        }

        #endregion
        #region Mouselook

        //grab the input from the mouse
        rotation.y += Input.GetAxis("Mouse Y");
        rotation.x += Input.GetAxis("Mouse X");

        //add rotation speeds to the mouse input
        rotY = rotation.x * lookSpeed;
        rotX = rotation.y * lookSpeed;

        // making sure we can't break our neck looking up
        rotX = Mathf.Clamp(rotX, minCamAngleX, maxCamAngleX); 
        
        //set angles as quaternions for rotation (better than using Euler angles)
        Quaternion playerRotation = Quaternion.Euler(0.0f, rotY, 0.0f);
        Quaternion cameraRotation = Quaternion.Euler(rotX, rotY, 0.0f);

        //apply the rotation
        transform.rotation = playerRotation;
        playerCamera.transform.rotation = cameraRotation;

        #endregion

        #region Jump
        if(Input.GetKeyDown(jump)){
            //apply upward force
            playerRB.AddForce(transform.up * jumpForce);

        }

        #endregion


    }
}

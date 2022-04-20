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

    //movement speeds & timers
    public float lookSpeed = 3.0f;
    public float forwardSpeed = 5.0f;
    public float strafeSpeed = 5.0f;
    public float jumpForce = 500.0f;

    int timer = 0;

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

    //default hand positions
    private Vector3 leftHandDefPos = new Vector3(-0.268f,0.324f,0.9f);
    private Vector3 rightHandDefPos = new Vector3(0.355f,0.324f,0.9f);

    private GameObject rightHand;
    private GameObject leftHand;

    //animations
    Animator m_Animator;

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

        //Get the animator and attach to Player
        m_Animator = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        #region Basic Movement

        if(Input.GetKey(forward)){
            //move forward
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
            m_Animator.SetBool("running", true);
        }

        if(Input.GetKey(back)){
            //move forward
            transform.Translate(Vector3.forward * -forwardSpeed * Time.deltaTime);
            m_Animator.SetBool("running", true);
        }

        if(Input.GetKey(right)){
            //move forward
            transform.Translate(Vector3.right * forwardSpeed * Time.deltaTime);
            m_Animator.SetBool("running", true);
        }

        if(Input.GetKey(left)){
            //move forward
            transform.Translate(Vector3.right * -forwardSpeed * Time.deltaTime);
            m_Animator.SetBool("running", true);
        }

        //check to see if the player is no longer moving and stop animation
        if(!Input.GetKey(forward) 
            && !Input.GetKey(back) 
            && !Input.GetKey(right) 
            && !Input.GetKey(left))
        {
            
            if(timer < 60){
                timer++;
            }else{
                m_Animator.SetBool("running", false);
                timer = 0;
            }


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

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

    private GameObject rightHand;
    private GameObject leftHand;

    //any other scripts we need
    [SerializeField] private GameManager gameManager;

    //animations
    Animator m_Animator;
    string defRunAnim = "running";
    string weapRunAnim = "runWeapon";
    string animHasWeap = "hasWeapon";
    string curAnimation;
    bool crRunning;
    bool areWeMoving;
    bool wereWeMoving;

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
        crRunning = false;
        wereWeMoving = false;
    }

    // Update is called once per frame
    void Update()
    {

        #region Basic Movement

        if (Input.GetKey(forward))
        {
            //move forward
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
            if (!areWeMoving)
            {
                areWeMoving = true;
            }
        }

        if (Input.GetKey(back))
        {
            //move forward
            transform.Translate(Vector3.forward * -forwardSpeed * Time.deltaTime);
            if (!areWeMoving)
            {
                areWeMoving = true;
            }
        }

        if (Input.GetKey(right))
        {
            //move forward
            transform.Translate(Vector3.right * forwardSpeed * Time.deltaTime);
            if (!areWeMoving)
            {
                areWeMoving = true;
            }
        }

        if (Input.GetKey(left))
        {
            //move forward
            transform.Translate(Vector3.right * -forwardSpeed * Time.deltaTime);
            if (!areWeMoving)
            {
                areWeMoving = true;
            }
        }

        //check to see if the player is no longer moving and stop animation
        if (!Input.GetKey(forward)
            && !Input.GetKey(back)
            && !Input.GetKey(right)
            && !Input.GetKey(left))
        {
                areWeMoving = false;
        }

        CheckAnimating();
        //Debug.Log(areWeMoving);

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
        if (Input.GetKeyDown(jump))
        {
            //apply upward force
            playerRB.AddForce(transform.up * jumpForce);

        }

        #endregion
    }
    #region Animating
    private void CheckAnimating()
    {

        //grab the current animation if it's playing
        string animName = CheckRunningAnimation();


        if (areWeMoving)
        {
            //if we weren't moving before, we are listed as moving now
            if(!wereWeMoving){
                wereWeMoving = true;
            }
            //if we are moving, is the coRoutine running?
            if (crRunning)
            {
                //stop the timer for the animation
                StopCoroutine("StopAnimating");
                crRunning = false;
            }

            //check to see if an animation is playing currently since we are moving
            if (animName != "null")
            { //there is an animation playing
                //did we pick up a weapon?
                if (gameManager.hasShooter && animName != weapRunAnim)
                {
                    Debug.Log(animName);
                    //stop the running animation, sending animation name and breakout function to stop immediately
                    StartCoroutine (StopAnimating(animName, true));

                    //start the weapon animation and set hasWeapon to true
                   m_Animator.SetBool(weapRunAnim, true);
                   m_Animator.SetBool(animHasWeap, true);
                }
                else if (gameManager.hasShooter && animName == weapRunAnim)
                {
                    //do nothing
                }
                else if (animName == defRunAnim && !gameManager.hasShooter)
                {
                    //do nothing
                }
            }
            else
            { // no animation is playing
                //do we have a weapon?
                if (gameManager.hasShooter)
                {
                    //set the variable to be the weapon animation
                    animName = weapRunAnim;
                    m_Animator.SetBool(animHasWeap, true);
                }
                else
                {
                    //set the variable to be the default running animation
                    animName = defRunAnim;
                    m_Animator.SetBool(animHasWeap, false);
                }

                //start animation
                m_Animator.SetBool(animName, true);
            }
            // if there have been no changes to the state of the animation we let it play
        }
        else
        { //if we stop moving start the co-routine & say that we aren't moving
            if(wereWeMoving){
            StartCoroutine (StopAnimating(animName, false));
            wereWeMoving = false;
            }
            //if we already weren't moving, do nothing
        }
    }

    IEnumerator StopAnimating(string curAnim, bool breakOut)
    {
        string stopAnim = curAnim;
        crRunning = true;
        //if we've not been asked to break the coroutine
        if (!breakOut)
        {
            for (float timer = 0; timer < 60; timer++)
            {
               // Debug.Log(timer);
                yield return new WaitForEndOfFrame();
            }
            breakOut = true;
           // Debug.Log("I broke out");
        }
        m_Animator.SetBool(stopAnim, false);
        crRunning = false;
    }

    string CheckRunningAnimation()
    {
        //are any of our known animation booleens true?
        if (m_Animator.GetBool(defRunAnim))
        {
            return defRunAnim;
        }
        else if (m_Animator.GetBool(weapRunAnim))
        {
            return weapRunAnim;
        }

        //if they are not, return nothing
        return "null";
    }

    #endregion
}

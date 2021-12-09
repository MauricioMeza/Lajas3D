using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool mobile;
    public CharacterController controller;
    public Transform playerCamera;
    float mouseSens = 200;
    float speed = 12;

    //Vars for movement
    Vector2 movement;
    Vector2 looking;
    float cameraY;

    //Vars for gravityCheck
    public Transform groundCheck;
    public LayerMask groundMask;
    public float gravity = -9.8f;
    float groundDistance = 0.4f;
    Vector3 velocity;
    bool isGrounded;

    //Vars of TouchControls
    int rightFId, leftFId;
    float halfScreenW;
    Vector2 moveTouchStartPos, moveInput, lookInput;

    // Start is called before the first frame update
    void Start()
    {
        if (mobile){
            leftFId = -1;
            rightFId = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mobile)
        {
            getTouchInput();
            mouseSens = 50;
            speed = 2;
        }
        else if(!mobile)
        {
            getKeyInput();
            getMouseInput();
            Cursor.lockState = CursorLockMode.Locked;
            mouseSens = 100;
            speed = 2.5f;
        }

        move();
        look();
    }


    //_______________________INPUTS________________________

    //Get inputs from touch in mobile
    void getTouchInput(){
        halfScreenW = Screen.width / 2;
        for (int i = 0; i < Input.touchCount; i++){
            Touch t = Input.GetTouch(i);

            switch (t.phase){
                case TouchPhase.Began:
                    if (t.position.x < halfScreenW && leftFId == -1){
                        leftFId = t.fingerId;
                        moveTouchStartPos = t.position;
                    }
                    else if (t.position.x > halfScreenW && rightFId == -1){
                        rightFId = t.fingerId;
                        moveTouchStartPos = t.position;
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (t.fingerId == leftFId){
                        leftFId = -1;
                        looking = Vector2.zero;
                    }
                    else if (t.fingerId == rightFId){
                        rightFId = -1;
                        movement = Vector2.zero;
                    }
                    break;
                case TouchPhase.Moved:
                    if (t.fingerId == leftFId){
                        looking = (t.position - moveTouchStartPos);
                        looking = Vector2.up * looking.x + Vector2.right * looking.y;
                    }
                    else if (t.fingerId == rightFId){
                        movement = (t.position - moveTouchStartPos);
                    }
                    break;
                case TouchPhase.Stationary:
                    if (t.fingerId == rightFId){
                        looking = Vector2.zero;
                    }
                    else if (t.fingerId == rightFId)
                    {
                        movement = Vector2.zero;
                    }
                    break;
            }
        }

    }

    //Get inputs from keyboard for player movements
    void getKeyInput(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        movement = Vector2.right * x + Vector2.up * y;
    }

    //Get inputs from mouse for camera rotations
    void getMouseInput(){
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        looking = Vector2.up * mouseX + Vector2.right * mouseY;
    }


    //_______________________MOVEMENTS________________________

    //Rotate from looking vector
    void look(){
        looking = looking.normalized;
        looking = looking * mouseSens * Time.deltaTime;
        cameraY = Mathf.Clamp(cameraY - looking.x, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(cameraY, 0, 0);
        transform.Rotate(transform.up, looking.y);
    }

    //Move from movement vector
    void move(){
        //Movement Check
        movement = movement.normalized;
        movement = movement * speed * Time.deltaTime;
        controller.Move(transform.right * movement.x + transform.forward * movement.y);

        //Gravity check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0){
            velocity.y = -1f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
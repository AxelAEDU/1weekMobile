using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class test : MonoBehaviour
{

    //GameComponents
    private Rigidbody Rb;

    //Jumping
    public float JumpH = 10f;
    public Transform GroundSpot;
    private bool IsGrounded = false;
    private bool Jumping = false;

    //Movment
    public float Speed = 0.2f;
    private PlayerInput PlayerInput;
    private InputAction Movement;
    private Vector2 MoveVec2;

    //Animator
    private Animator Animator;
    private bool IsRunning = false;


    public void OnEnable()
    {
        PlayerInput = GetComponent<PlayerInput>();
        Movement = PlayerInput.actions["Move"];
    }

    private void Awake()
    {
        //Changes the gravity
        Physics.gravity = new Vector3(0, -100f, 0);
        //Getting the Animator
        Animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        //Getting the Rigidbody
        Rb = GetComponent<Rigidbody>();
        //Getting the Checkpoint
        CheckPoints();
    }

    void CheckPoints()
    {
        //Player will spawn at last checkpoint
    }



    private void FixedUpdate()
    {
        //calling on Move
        Move();
        //calling on Jump
        Jump();
        //calling on WalkingAnimation
        AnimationWalking();
    }

    void AnimationWalking()
    {
        //Getting infomation from Animatorn to play IsRunning animation
        IsRunning = (MoveVec2.x > 0.1f || MoveVec2.x < -0.1f) ? true : false;
        Animator.SetBool("IsRunning", IsRunning);
    }

    private void Move()
    {
        //Make the charater move
        MoveVec2 = Movement.ReadValue<Vector2>();
        MoveVec2.y = 0;
        Rb.velocity = MoveVec2.normalized * Speed;

        //Rotate the charater in the direction you move
        if (MoveVec2.x < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
        }
        else if (MoveVec2.x > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }
    }

    private void Jump()
    {
        //Will check if IsGrounded and Jumping are true
        if (Jumping == true && IsGrounded == true)
        {
            //You will jump
            Rb.velocity = new Vector3(0, JumpH, 0);
            Jumping = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Will check if IsGrounded = true
        if (IsGrounded == true)
        {
            //Will check if Jumping = true
            Jumping = true;
        }
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interacting");
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Cheacking if you are in collision with an object taged "Ground"
        if (collision.gameObject.tag == "Ground")
        {
            //Checking if its true that you are on the ground
            IsGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Cheacking if you are not in collision with an object taged "Ground"
        if (collision.gameObject.tag == "Ground")
        {
            //Checking if you are not on the ground
            IsGrounded = false;
        }
    }
}

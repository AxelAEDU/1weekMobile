using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //GameComponents
    private Rigidbody rB;
    private UserPlayer _userPlayer;

    //Jumping
    public float JumpH = 10f;
    public Transform GroundSpot;
    private bool IsGrounded = false;
    private bool Jumping = false;

    //Movment
    private Vector2 Movements;
    public float speed = 10f;
    private bool isFacingRight = true;

    //Sprint
    public float sprintSpeed = 10f;
    public bool isSprinting;



    //Animator
    private Animator anim;
    private bool _isRunning = false;

    public void OnEnable()
    {        
        _userPlayer.Player.Enable();
    }

    public void OnDisable()
    {
        _userPlayer.Player.Disable();
    }

    private void Awake()
    {
        _userPlayer = new UserPlayer();
        _userPlayer.Player.SprintStart.performed += x => SprintPressed();
        _userPlayer.Player.SprintDone.performed += x => SprintReleased();
        Physics.gravity = new Vector3(0, -400f, 0);
    }
    void Start()
    {
        //Getting the Animator
        anim = gameObject.GetComponentInChildren<Animator>();
        //Getting the Rigidbody
        rB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //calling on Animation
        Animation();
        //calling on FacingRightDirection
        FacingRightDirection();
    }
    private void FixedUpdate()
    {
        //calling on Move
        Movment();
        //calling on Jump
        Jump();
        //Sprint();
    }

    void Animation()
    {
        //Getting infomation from Animatorn to play running animation
        _isRunning = (Movements.x > 0.1f || Movements.x < -0.1f) ? true : false;
        anim.SetBool("isRunning", _isRunning);

    }

    void FacingRightDirection()
    {
        //Getting infomation on what direction player are moving
        rB.velocity = new Vector2(Movements.x * speed, rB.velocity.y);

        if (!isFacingRight && Movements.x > 0f)
        {
            Flip();
        }
        else if (isFacingRight && Movements.x < 0f)
        {
            Flip();
        }
    }

    private void Flip()
    {
        //Rotate the charater in the direction you move
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Movment()
    {
        //make the player move
        Movements = _userPlayer.Player.Move.ReadValue<Vector2>();
        
        if (isSprinting)
        {
            rB.velocity = sprintSpeed * Movements.y * transform.forward + (transform.right * Movements.x) * sprintSpeed;
        }
        else
        {
            rB.velocity = speed * Movements.y * transform.forward + (transform.right * Movements.x) * speed;
        }
    }
    private void Jump()
    {
        //Will check if IsGrounded and Jumping are true
        if (Jumping == true && IsGrounded == true)
        {
            ////You will jump
            rB.velocity = new Vector3(0, JumpH, 0);
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
    private void SprintPressed()
    {
        isSprinting = true;
    }
    private void SprintReleased()
    {
        isSprinting = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Checking if you are in collision with an object tagged "Ground"
        if (collision.gameObject.tag == "Ground")
        {
            //Checking if its true that you are on the ground
            IsGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Checking if you are not in collision with an object tagged "Ground"
        if (collision.gameObject.tag == "Ground")
        {
            //Checking if you are not on the ground
            IsGrounded = false;
        }
    }
}

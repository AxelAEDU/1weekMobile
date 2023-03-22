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
    public float maxJumpHeight = 10f;
    public Transform groundCheck;
    private bool _isGrounded = false;
    private bool _isJumping = false;
    public float fallMultiplier = 1.5f;

    float _initialJumpVelocity;
    float _maxJumpTime;

    //Movment
    private Vector2 _movements;
    public float speed = 10f;
    private bool _isFacingRight = true;

    //Sprint
    public float sprintSpeed = 10f;
    public bool isSprinting;

    //Gravity
    float _gravity = -250f;
    float _groundGravity = -0.05f;



    //Animator
    private Animator _animator;
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
    }
    void Start()
    {
        //Getting the Animator
        _animator = gameObject.GetComponentInChildren<Animator>();
        //Getting the Rigidbody
        rB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //calling on FacingRightDirection
        FacingRightDirection();
        //calling on Animation
        Animation();
        //calling on Move
        Movment();
        //calling on Gravity
        HandleGravity();
    }
    private void FixedUpdate()
    {
        //calling on Jump
        Jump();
    }

    void Animation()
    {
        //Getting infomation from Animatorn to play running animation
        _isRunning = (_movements.x > 0.1f || _movements.x < -0.1f) ? true : false;
        _animator.SetBool("isRunning", _isRunning);

    }

    void FacingRightDirection()
    {
        //Getting infomation on what direction player are moving
        rB.velocity = new Vector2(_movements.x * speed, rB.velocity.y);

        if (!_isFacingRight && _movements.x > 0f)
        {
            Flip();
        }
        else if (_isFacingRight && _movements.x < 0f)
        {
            Flip();
        }
    }

    private void Flip()
    {
        //Rotate the charater in the direction you move
        _isFacingRight = !_isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Movment()
    {
        //make the player move
        _movements = _userPlayer.Player.Move.ReadValue<Vector2>();
        
        if (isSprinting)
        {
            //if Sprint button are pressed then the Sprinting = true
            rB.velocity = sprintSpeed * _movements.y * transform.forward + (transform.right * _movements.x) * sprintSpeed;
        }
        else
        {
            //if Sprint button are released then the Sprinting = false
            rB.velocity = speed * _movements.y * transform.forward + (transform.right * _movements.x) * speed;
        }
    }
    private void Jump()
    {
        //Will check if IsGrounded and Jumping are true
        if (_isJumping == true && _isGrounded == true)
        {
            ////You will jump
            rB.velocity = new Vector3(rB.velocity.x, maxJumpHeight, 0);
            _isJumping = false;
        }

    }



    public void OnJump(InputAction.CallbackContext context)
    {
        //Will check if IsGrounded = true
        if (context.performed && _isGrounded == true)
        {
            //Will check if Jumping = true
            _isJumping = true;
        }
    }

    void HandleGravity()
    {
        if (_isGrounded)
        {
            _movements.y = _groundGravity;
        }
        else
        {
            _movements.y += _gravity * Time.deltaTime;
        }
    }
    private void SprintPressed()
    {
        //Making the Sprinting = true
        isSprinting = true;
    }
    private void SprintReleased()
    {
        //Making the Sprinting = false
        isSprinting = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Checking if you are in collision with an object tagged "Ground"
        if (collision.gameObject.tag == "Ground")
        {
            //Checking if its true that you are on the ground
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Checking if you are not in collision with an object tagged "Ground"
        if (collision.gameObject.tag == "Ground")
        {
            //Checking if you are not on the ground
            _isGrounded = false;
        }
    }
}

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
    private float jumpingPower = 100f;
    public LayerMask groundLayer;
    private Vector3 _boxSize;
    private float _maxDistance = 1;

    //Movment
    private Vector2 Movements;
    private float speed = 10f;
    private bool isFacingRight = true;



    //Animator
    private Animator anim;
    private bool _isRunning = false;

    public void OnEnable()
    {
        _userPlayer = new UserPlayer();
        _userPlayer.Player.Enable();
    }

    public void OnDisable()
    {
        _userPlayer.Player.Disable();
    }

    private void Awake()
    {
        Physics.gravity = new Vector3(0, -300f, 0);
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
        Movment();
        Jump();

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

    public bool IsGrounded()
    {
        //Will check if you are Grounded
        if (Physics.BoxCast(transform.position, _boxSize, -transform.up, transform.rotation, _maxDistance, groundLayer))
        {
            Debug.Log("Grounded");
            return true;
        }
        else
        {
            Debug.Log("NotGrounded");
            return false;
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
        Movements = _userPlayer.Player.Move.ReadValue<Vector2>();
        rB.velocity = speed * Movements.y * transform.forward + (transform.right * Movements.x) * speed;
    }

    public void Jump()
    {
        if (_userPlayer.Player.Jump.triggered && IsGrounded())
        {
            Debug.Log("jumping");
            rB.velocity = new Vector2(rB.velocity.x, jumpingPower);
        }




    }



    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("fire0");
    }
}

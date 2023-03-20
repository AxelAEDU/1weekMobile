using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rB;
    public LayerMask groundLayer;
    private Vector3 _boxSize;
    private float _maxDistance = 2;

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 8f;
    private bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rB.velocity = new Vector2(horizontal * speed, rB.velocity.y);

        if(!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
    }

    private bool IsGrounded()
    {
        if (Physics.BoxCast(transform.position, _boxSize, -transform.up, transform.rotation, _maxDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && IsGrounded())
        {
            rB.velocity = new Vector2(rB.velocity.x, jumpingPower);
        }
        if(context.canceled && rB.velocity.y > 0f)
        {
            rB.velocity = new Vector2(rB.velocity.x, rB.velocity.y * 0.5f);
        }
    }
}

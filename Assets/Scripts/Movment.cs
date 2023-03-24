using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movment : MonoBehaviour
{
    // declare reference variables
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    //variables to store optimized setter/getter parameter IDs
    int isRunningHash;
    int isWalkingHash;
    int isJumpingHash;
    int isDeadHash;

    //variables to store player input values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;
    float movementSpeed = 5;
    bool isMovementPressed;
    bool isRunPressed;
    float rotationFactorPerFrame = 10f;
    float runMultiplier = 2f;


    //Gravity variables
    float gravity = -9.8f;
    float groundGravity = -0.05f;

    //jumping variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 1f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;
    bool isJumpAnimating = false;





    private void OnEnable()
    {
        playerInput.Player.Enable();
    }
    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
    private void Awake()
    {
        //set reference variables
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        //parameter animation hash references
        isRunningHash = Animator.StringToHash("isRunning");
        isWalkingHash = Animator.StringToHash("isWalking");
        isJumpingHash = Animator.StringToHash("isJumping");
        isDeadHash = Animator.StringToHash("isDead");

        //player input callbacks
        playerInput.Player.Move.started += OnMovementInput;
        playerInput.Player.Move.canceled += OnMovementInput;
        playerInput.Player.Move.performed += OnMovementInput;
        playerInput.Player.Run.started += OnRun;
        playerInput.Player.Run.canceled += OnRun;
        playerInput.Player.Jump.started += OnJump;
        playerInput.Player.Jump.canceled += OnJump;

        setupJumpVariables();

    }

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        handleAnimation();
        // moving the player
        if (isRunPressed)
        {
            appliedMovement.x = currentRunMovement.x;
        }
        else
        {
            appliedMovement.x = currentMovement.x;
        }

        characterController.Move(appliedMovement * movementSpeed * Time.deltaTime);

        handleGravity();
        handleJump();

    }


    void setupJumpVariables()
    {
        //handling the jump variables
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleJump()
    {
        // handling the jump mecanic
        if(!isJumping && characterController.isGrounded && isJumpPressed)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = initialJumpVelocity;
            appliedMovement.y = initialJumpVelocity;
        }
        else if(!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0f || !isJumpPressed;
        float fallMultiplier = 2.0f;
        //apply gravity depending idf the player is grounded or not
        if (characterController.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
            }

            currentMovement.y = groundGravity;
            appliedMovement.y = groundGravity;
        }
        // additional gravity applied after reaching apex of jump
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max ((previousYVelocity + currentMovement.y) * .5f,- 20f);
        }
        // when charater is not grounded
        else
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (gravity * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + currentMovement.y) * .5f;
        }
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        // the change in the position our player should look at
        positionToLookAt.x = currentMovement.z;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.x;
        //the current rotation of our player
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            //new raotation based on the player current movement press
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }

    }

    void handleAnimation()
    {
        //get parameter value from animator
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);

        // start walkmovement animation if movement pressed is true and not moveing
        if(isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        // stop walkmovement animation if movement pressed is false and not moveing
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }
        // start runmovement animation if movement pressed is true and not moveing
        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        // stop runmovement animation if movement pressed is false and not moveing
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0;
    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    private void OnTriggerEnter(Collider other)
    {
        //plater will get killed if he collied with the TAG killbox
        if (other.tag == "KillBox")
        {
            animator.SetBool(isDeadHash, true);
            characterController.enabled = false;       
        }
    }
}

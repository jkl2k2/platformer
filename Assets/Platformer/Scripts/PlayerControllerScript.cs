using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerScript : MonoBehaviour
{
    // Components
    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;

    // Movement vectors
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    
    // Button press variables
    private bool isMovementPressed;
    private bool isRunPressed;
    private bool isJumpPressed;

    // Gravity constants
    private float gravity = -9.8f * 0.25f;
    private const float groundedGravity = -0.05f;
    
    // Jump constants
    private float initialJumpVelocity;
    private const float maxJumpHeight = 1f;
    private const float maxJumpTime = 0.5f;
    private bool isJumping = false;

    // Run constant
    private const float runMultiplier = 3f;

    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;
        
        setupJumpVariables();
    }

    private void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    private void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            isJumping = true;
            animator.SetBool("Jumping", true);
            currentMovement.y = initialJumpVelocity * 0.5f;
            currentRunMovement.y = initialJumpVelocity * 0.5f;
        } else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
            animator.SetBool("Jumping", false);
        }
    }

    private void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    
    private void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }
    
    private void handleGravity()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }
    }

    private void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        
        currentMovement.x = currentMovementInput.x;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        
        isMovementPressed = currentMovementInput.x != 0;
    }

    private void Update()
    {
        animator.SetFloat("Speed", Math.Abs(currentMovement.x));

        if (currentMovementInput.x == -1)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (currentMovementInput.x == 1)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        
        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
            animator.SetBool("Running", true);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
            animator.SetBool("Running", false);
        }
        
        handleGravity();
        handleJump();
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}

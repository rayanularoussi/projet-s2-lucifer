using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;
    PlayerStats playerStats;

    public Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rb;

    Transform myTransform;
    Vector3 targetPosition;
    Vector3 normalVector;
    float movementSpeed;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isInAir;
    public bool isOnGround;
    
    [Header("Movement Speeds")]
    public float sprintingSpeed = 5f;
    public float runningSpeed = 2f;
    public float walkingSpeed = 0.5f;
    public float rotationSpeed = 10;
    float fallingSpeed = 45;

    [Header("Fall")]
    [SerializeField]
    float groundDetectionRayStartPoint = 0.5f;
    [SerializeField]
    float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField]
    float groundDirectioNRayDistance = 0.2f;
    LayerMask ignoreForGroundCheck;
    public float inAirTimer;

    [HideInInspector]
    public AnimatorHandler animatorHandler;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        cameraObject = Camera.main.transform;
        animatorHandler.Awake();
        myTransform = transform;

        isOnGround = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
    }

    public void HandleMovement()
    {   
        if(playerManager.isInteracting)
        {
            return;
        }
        
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.y = 0;
        moveDirection.Normalize();

        //If we are sprinting, we select sprinting speed
        //If we are running, we select running speed
        //Then if we are walking, we select walking speed
        if(isSprinting)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else 
        {
            if(inputManager.moveAmount >= 0.5f)
            {
                moveDirection = moveDirection * runningSpeed;
            }
            else
            {
                moveDirection = moveDirection * walkingSpeed;
            }
        }
        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;

    }

    public void HandleRotation()
    {
        if(playerStats.currentHealth == 0)
        {
            return;
        }

        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public void HandleRolling()
    {
        if(animatorHandler.animator.GetBool("isInteracting"))
        {
            return;
        }

        if(inputManager.rollAvailable)
        {
            moveDirection = cameraObject.forward * inputManager.verticalInput;
            moveDirection += cameraObject.right * inputManager.horizontalInput;
            if(inputManager.moveAmount > 0 && isSprinting)
            {
                animatorHandler.PlayTargetAnimation("RollingForwardFast", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else if(inputManager.moveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("RollingForward", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else
            {
                animatorHandler.PlayTargetAnimation("RollingBackward", true);
            }
        }
    }

    public void SetIsInteractingTrue()
    {
       playerManager.isInteracting = true;
    }

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        isOnGround = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y = origin.y + groundDetectionRayStartPoint;

        if(Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if(playerManager.isInAir)
        {
            rb.AddForce(-Vector3.up * fallingSpeed * 5);
            rb.AddForce(moveDirection * fallingSpeed / 5f);
        }

        Vector3 direction = moveDirection;
        direction.Normalize();
        origin = origin + direction * groundDirectioNRayDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if(Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            isOnGround = true;
            targetPosition.y = tp.y;

            if(playerManager.isInAir)
            {
                if(inAirTimer > 0.5f)
                {
                    Debug.Log("You were in the air for " + inAirTimer);
                    animatorHandler.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Empty", false);
                    inAirTimer = 0;
                }

                playerManager.isInAir = false;
            }
        }
        else
        {
            if(isOnGround)
            {
                isOnGround = false;
            }

            if(playerManager.isInAir == false)
            {
                if(playerManager.isInteracting == false)
                {
                    animatorHandler.PlayTargetAnimation("Falling", true);
                }

                Vector3 vel = rb.velocity;
                vel.Normalize();
                rb.velocity = vel * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }
       
       if(isOnGround)
       {
            if(playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = targetPosition;
            }
       }
    }
}

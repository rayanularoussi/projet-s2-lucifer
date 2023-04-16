using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocomotion playerLocomotion;
    Animator animator;

    public PhotonView view;

    [Header("Player phase")]
    public bool isInAir;
    public bool isInteracting;
    public bool canDoCombo;

    private void Awake()
    {
        float delta = Time.deltaTime;
        
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
        view = GetComponent<PhotonView>(); 
    }

    private void Update()
    {
        isInteracting = animator.GetBool("isInteracting");
        inputManager.rollAvailable = false;
        if(view.IsMine)
        {
            inputManager.HandleAllInputs();
        }
        CheckForInteractableObject();
    }

    public void HandleAllMovement()
    {
        float delta = Time.deltaTime;
        isInteracting = animator.GetBool("isInteracting");
        canDoCombo = animator.GetBool("canDoCombo");
        
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        playerLocomotion.HandleMovement();
        playerLocomotion.HandleRotation();
        playerLocomotion.HandleRolling();  
    }

    private void FixedUpdate()
    {
        HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
        inputManager.rollAvailable = false;
        inputManager.la_Input = false;
        inputManager.ha_Input = false;
        inputManager.d_Pad_Down = false;
        inputManager.d_Pad_Left = false;
        inputManager.d_Pad_Right = false;
        inputManager.d_Pad_Up = false;
        inputManager.interact_Input = false;
        inputManager.inventory_Input = false;
        
        isInteracting = animator.GetBool("isInteracting");

        if(isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }
    }

    public void CheckForInteractableObject()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position;
        rayOrigin.y = rayOrigin.y + 2f;
        
        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraManager.collisionLayers) ||
                Physics.SphereCast(rayOrigin, 0.3f, Vector3.down, out hit, 2.5f, cameraManager.collisionLayers))
        {
            if(hit.collider.tag == "Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if(interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    //SET THE UI TEXT TO THE INTERACTABLE OBJECT'S TEXT
                    //SET THE TEXT POP UP TO TRUE

                    if(inputManager.interact_Input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
    }
}

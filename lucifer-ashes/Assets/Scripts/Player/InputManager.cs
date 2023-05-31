using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    Dashing dashing;
    AnimatorHandler animatorHandler;
    PlayerManager playerManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    [Header("Inputs")]
    public bool s_Input; //sprint bool

    public bool r_Input; //roll bool
    public bool rollAvailable;
    
    public bool d_Input; //dash bool

    public bool la_Input; //Light attack bool
    public bool ha_Input; //Heavy attack bool

    public bool comboFlag;
    public bool inventoryFlag;

    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Right;
    public bool d_Pad_Left;

    public bool interact_Input;

    public bool inventory_Input;

    


    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;

    UIManager uiManager;
    
    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        dashing = GetComponent<Dashing>();
        animatorHandler = GetComponent<AnimatorHandler>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>(); //Sending the bindings to the vector2 variable
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Sprint.performed += i => s_Input = true;
            playerControls.PlayerActions.Sprint.canceled += i => s_Input = false;

            playerControls.PlayerActions.Dash.performed += i => d_Input = true;
            playerControls.PlayerActions.Dash.canceled += i => d_Input = false;

            playerControls.PlayerActions.Roll.performed += i => r_Input = true;
            playerControls.PlayerActions.Roll.canceled += i => r_Input = false;

            
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleDashingInput();
        HandleRollInput();
        HandleAttackInput();
        HandleQuickSlotsInput();
        HandleInteractingButtonInput();
        HandleInventoryInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorHandler.UpdateAnimatorValues(0, moveAmount, s_Input);

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
    }

    private void HandleSprintingInput()
    {
        if(s_Input && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleDashingInput()
    {
        if(d_Input)
        {
            dashing.isDashing = true;
        }
        else
        {
            dashing.isDashing = false;
        }
    }

    private void HandleRollInput()
    {
        if (r_Input)
        {
            rollAvailable = true;
        }
        if (rollAvailable)
        {
            playerLocomotion.HandleRolling();
            rollAvailable = false;
        }
    }

    private void HandleAttackInput()
    {
        if(!playerManager.view.IsMine)
        {
            return;
        }

        if(playerManager.isInteracting) return;

        playerControls.PlayerActions.Light_Attack.performed += i => la_Input = true;
        playerControls.PlayerActions.Heavy_Attack.performed += i => ha_Input = true;

        if (la_Input)
        {
            if(playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if(playerManager.isInteracting)
                {
                    return;
                }
                if(playerManager.canDoCombo)
                {
                    return;
                }
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
            
        }

        if (ha_Input)
        {
            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            ha_Input = false;
        }
    }

    private void HandleQuickSlotsInput()
    {
        playerControls.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
        playerControls.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
        if(d_Pad_Right)
        {
            playerInventory.ChangeRightWeapon();
        }
        else if(d_Pad_Left)
        {
            playerInventory.ChangeLeftWeapon();
        }
    }

    private void HandleInteractingButtonInput()
    {
        playerControls.PlayerActions.Interact.performed += i => interact_Input = true;
    }

    private void HandleInventoryInput()
    {
        playerControls.PlayerActions.Inventory.performed += i => inventory_Input = true;

        if(inventory_Input)
        {
            inventoryFlag = !inventoryFlag;

            if(inventoryFlag)
            {
                uiManager.OpenSelectWindow();
                uiManager.UpdateUI();
                uiManager.hudWindow.SetActive(false);
            }
            else
            {
                uiManager.CloseSelectWindow();
                uiManager.CloseAllInventoryWindows();
                uiManager.hudWindow.SetActive(true);
            }
        }
    }
}
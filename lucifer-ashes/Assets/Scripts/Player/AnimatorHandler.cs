using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : AnimatorManager
{
    PlayerManager playerManager;
    public InputManager inputManager;
    public PlayerLocomotion playerLocomotion;
    int vertical;
    int horizontal;



    public void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        inputManager = GetComponentInParent<InputManager>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical
        float v = 0;
        
        if(verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1;
        }
        else if(verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }
        #endregion



        #region Horizontal
        float h = 0;

        if(horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1;
        }
        else if(horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }
        #endregion

        if(isSprinting && (verticalMovement != 0 || horizontalMovement != 0))
        {
            h = horizontalMovement;
            v = 2;
        }

        animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    private void OnAnimatorMove()
    {
        if(playerManager.isInteracting != false)
        {
            float delta = Time.deltaTime;
            playerLocomotion.GetComponent<Rigidbody>().drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.GetComponent<Rigidbody>().velocity = velocity;
        }
    }
}

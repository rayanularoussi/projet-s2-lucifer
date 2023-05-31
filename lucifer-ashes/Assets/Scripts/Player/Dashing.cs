using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform playerCam;
    private Rigidbody rb;
    PlayerLocomotion playerLocomotion;
    PlayerControls playerControls;

    [Header("Dashing")]
    public float dashForce;
    public float dashDuration;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public bool isDashing;

    private bool isDashingInProgress = false;
    private float dashTimeElapsed = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
        else
        {
            if (isDashing && !isDashingInProgress)
            {
                isDashingInProgress = true;
                dashTimeElapsed = 0f;
                dashCdTimer = dashCd;
            }
        }

        if (isDashingInProgress)
        {
            Dash();
        }
    }

    private void Dash()
    {
        float dashCompletionRatio = dashTimeElapsed / dashDuration;
        Vector3 forceToApply = player.forward * dashForce * 10;
        float interpolatedForceMagnitude = Mathf.Lerp(0, forceToApply.magnitude, dashCompletionRatio);
        Vector3 interpolatedForce = interpolatedForceMagnitude * player.forward;

        rb.AddForce(interpolatedForce, ForceMode.Impulse);
        dashTimeElapsed += Time.deltaTime;

        if (dashTimeElapsed >= dashDuration)
        {
            ResetDash();
        }
    }

    private void ResetDash()
    {
        isDashingInProgress = false;
    }
}
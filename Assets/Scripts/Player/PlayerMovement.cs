using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerInputManager playerInputManager;
    AnimatorManager animatorManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("Falling")]
    public float inAirTime;
    public float leapingVelocity;
    public float fallingVelocity;
    public float raycastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    [Header("Jump")]
    public float jumpHeight = 2;
    public float gravityIntensity = -20;

    [Header("Movement Bools")]
    public bool isGrounded;
    public bool isJumping;
    public bool isAttacking;

    [Header("Movement Speeds")]
    public float movementSpeed = 7f;
    public float rotationSpeed = 15f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerInputManager = GetComponent<PlayerInputManager>();
        animatorManager = GetComponent<AnimatorManager>();

        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;

        isGrounded = true;
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (playerManager.isInteracting || isAttacking)
            return;
        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {
        if (isAttacking)
            return;

        moveDirection = cameraObject.forward * playerInputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * playerInputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        if (isAttacking)
            return;

        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * playerInputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * playerInputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y = raycastOrigin.y + raycastHeightOffset;
        Vector3 targetPosition = transform.position;

        if (!isGrounded && !isJumping && !isAttacking)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Fall", true);
            }

            inAirTime = inAirTime + Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(Vector3.down * fallingVelocity * inAirTime);

        }

        if (Physics.SphereCast(raycastOrigin, 0.2f, Vector3.down, out hit, 0.6f, groundLayer))
        {
            if (!isGrounded &&  playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTime = 0;
            isGrounded = true;

            playerManager.isInteracting = false;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping)
        {
            if(playerManager.isInteracting || playerInputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJump()
    {
        if (isGrounded && !isAttacking)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playervelocity = moveDirection;
            playervelocity.y = jumpingVelocity;

            playerRigidbody.velocity = playervelocity;
        }
    }

    public void HandleAttack()
    {
        if (isGrounded && !isJumping )
        {
            animatorManager.animator.SetBool("isAttacking", true); 
            StartCoroutine(ResetIsAttacking());
        }
    }

    private IEnumerator ResetIsAttacking()
    {
        yield return new WaitForSeconds(0.5f);
        animatorManager.animator.SetBool("isAttacking", false);
    }
}

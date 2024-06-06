using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    CameraManager cameraManager;
    Animator animator;
    PlayerMovement playerMovement;

    public bool isInteracting;
    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        playerInputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerMovement.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        playerMovement.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerMovement.isGrounded);
    }
}

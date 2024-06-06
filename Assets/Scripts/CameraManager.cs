using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    public LayerMask collisionLayers;

    public Transform targetTransform;
    public Transform cameraPivot;
    public Transform cameraTransform;

    private float defaultPosition;
    private Vector3 defaultCameraLocalPosition;

    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    private float cameraCollisionOffset = 0.2f;
    private float minCollisionOffset = 0.2f;
    public float cameraCollisionRadius = 0.2f;
    public float camFollowSpeed = 0.1f;
    public float camPivotSpeed = 0.2f;

    public float lookAngle;
    public float pivotAngle;
    public float minPivotAngle = -50;
    public float maxPivotAngle = 50;

    public SettingsData settingsData;
    public float sensitivity;
    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        defaultPosition = cameraTransform.localPosition.z;
        defaultCameraLocalPosition = cameraTransform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        sensitivity = settingsData.sensitivity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, camFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        lookAngle += playerInputManager.cameraInputX * sensitivity;
        pivotAngle -= playerInputManager.cameraInputY * camPivotSpeed;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        transform.rotation = Quaternion.Euler(0, lookAngle, 0);
        cameraPivot.localRotation = Quaternion.Euler(pivotAngle, 0, 0);
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minCollisionOffset)
        {
            targetPosition -= minCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}

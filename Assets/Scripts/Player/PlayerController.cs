using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody _rigidBody;
    public bool PlayerCanMove = true;

    private PlayerCameraController playerCameraController;
    private JumpController jumpController;
    private CrouchController crouchController;
    private SprintController sprintController;
    private HeadBobController headBobController;
    private DashController dashController;

    private void Awake()
    {
        jumpController = GetComponent<JumpController>();
        crouchController = GetComponent<CrouchController>();
        sprintController = GetComponent<SprintController>();
        headBobController = GetComponent<HeadBobController>();
        playerCameraController = GetComponent<PlayerCameraController>();
        dashController = GetComponent<DashController>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (PlayerCanMove)
        {
            jumpController.UpdateIsGrounded();
            playerCameraController.UpdatePlayerCameraState();
            playerCameraController.UpdateZoomState();
            sprintController.UpdateSprintStates();
            jumpController.UpdateJumpState();
            crouchController.UpdateCrouchState();
            headBobController.UpdateHeadBobState();
            dashController.UpdateDashState();
        }
    }

    private void FixedUpdate()
    {
        if (PlayerCanMove)
        {
            sprintController.UpdateSprintMovementState();
            
        }
            
    }

}
using UnityEngine;

public class JumpController : MonoBehaviour
{

    public bool EnableJump = true;
    public bool EnableDoubleJump = true;
    public bool IsGrounded = false;
    public float DoubleJumpPower = 5f;
    public float JumpPower = 5f;
    public KeyCode JumpKey = KeyCode.Space;
    public bool hasDoubleJumped = false;
    private PlayerController firstPersonController;
    private CrouchController crouchController;

    private void Start()
    {
        firstPersonController = GetComponent<PlayerController>();
        crouchController = GetComponent<CrouchController>();
    }

    public void UpdateIsGrounded()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        float distance = .75f;
        if (Physics.Raycast(origin, transform.TransformDirection(Vector3.down), out RaycastHit hit, distance))
        {
            //Debug.DrawRay(origin, direction * distance, Color.red);
            IsGrounded = true;
            hasDoubleJumped = false;
        }
        else IsGrounded = false;
    }

    private void FirstJump()
    {
        if (IsGrounded)
        {
            firstPersonController._rigidBody.AddForce(0f, JumpPower, 0f, ForceMode.Impulse);
            IsGrounded = false;
        }

        if (crouchController.IsCrouched && !crouchController.HoldToCrouch)
            crouchController.Crouch();
    }

    private void DoubleJump()
    {
        if (!IsGrounded && !hasDoubleJumped)
        {
            firstPersonController._rigidBody.velocity = new Vector3(firstPersonController._rigidBody.velocity.x, 0f, firstPersonController._rigidBody.velocity.z);
            firstPersonController._rigidBody.AddForce(0f, DoubleJumpPower, 0f, ForceMode.Impulse);
            hasDoubleJumped = true;
        }
    }

    public void UpdateJumpState()
    {
        if (EnableDoubleJump && Input.GetKeyDown(JumpKey) && IsGrounded && EnableJump)
            FirstJump();
        else if (EnableDoubleJump && Input.GetKeyDown(JumpKey) && !IsGrounded && EnableJump)
            DoubleJump();
    }

}

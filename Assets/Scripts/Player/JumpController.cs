using UnityEngine;

public class JumpController : MonoBehaviour
{

    public bool EnableDoubleJump = true;
    public float DoubleJumpPower = 5f;
    public float JumpPower = 5f;
    public KeyCode JumpKey = KeyCode.Space;
    public bool hasDoubleJumped = false;
    public FirstPersonController FirstPersonController;

    private void FirstJump()
    {
        if (FirstPersonController.isGrounded)
        {
            FirstPersonController.rb.AddForce(0f, JumpPower, 0f, ForceMode.Impulse);
            FirstPersonController.isGrounded = false;
        }

        // When crouched and using toggle system, will uncrouch for a jump
        if (FirstPersonController.CrouchController.IsCrouched && !FirstPersonController.CrouchController.HoldToCrouch)
            FirstPersonController.CrouchController.Crouch();
    }

    private void DoubleJump()
    {
        if (!FirstPersonController.isGrounded && !hasDoubleJumped)
        {
            FirstPersonController.rb.velocity = new Vector3(FirstPersonController.rb.velocity.x, 0f, FirstPersonController.rb.velocity.z);
            FirstPersonController.rb.AddForce(0f, DoubleJumpPower, 0f, ForceMode.Impulse);
            hasDoubleJumped = true;
        }
    }

    public void Jump()
    {
        if (EnableDoubleJump && Input.GetKeyDown(JumpKey) && FirstPersonController.isGrounded)
            FirstJump();
        else if (EnableDoubleJump && Input.GetKeyDown(JumpKey) && !FirstPersonController.isGrounded)
            DoubleJump();
    }

    public void Update()
    {
        Jump();
    }
}

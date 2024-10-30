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
    private FirstPersonController firstPersonController;

    private void Start()
    {
        firstPersonController = GetComponent<FirstPersonController>();
    }

    private void SetIsGrounded()
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
            firstPersonController.rb.AddForce(0f, JumpPower, 0f, ForceMode.Impulse);
            IsGrounded = false;
        }

        if (firstPersonController.CrouchController.IsCrouched && !firstPersonController.CrouchController.HoldToCrouch)
            firstPersonController.CrouchController.Crouch();
    }

    private void DoubleJump()
    {
        if (!IsGrounded && !hasDoubleJumped)
        {
            firstPersonController.rb.velocity = new Vector3(firstPersonController.rb.velocity.x, 0f, firstPersonController.rb.velocity.z);
            firstPersonController.rb.AddForce(0f, DoubleJumpPower, 0f, ForceMode.Impulse);
            hasDoubleJumped = true;
        }
    }

    public void Jump()
    {
        if (EnableDoubleJump && Input.GetKeyDown(JumpKey) && IsGrounded && EnableJump)
            FirstJump();
        else if (EnableDoubleJump && Input.GetKeyDown(JumpKey) && !IsGrounded && EnableJump)
            DoubleJump();
    }

    public void Update()
    {
        SetIsGrounded();
        if (EnableJump)
            Jump();
    }
}

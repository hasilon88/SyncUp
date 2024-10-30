using UnityEngine;

public class HeadBobController : MonoBehaviour
{

    public bool EnableHeadBob = true;
    public Transform Joint;
    public float BobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(0f, .15f, 0f);
    private Vector3 jointOriginalPos;
    private float timer = 0;
    private SprintController sprintController;
    private CrouchController crouchController;

    private void Awake()
    {
        sprintController = GetComponent<SprintController>();
        crouchController = GetComponent<CrouchController>();
        jointOriginalPos = Joint.localPosition;
    }

    private void HeadBob()
    {
        if (sprintController.IsWalking)
        {
            if (sprintController.IsSprinting) // Calculates HeadBob speed during sprint
                timer += Time.deltaTime * (BobSpeed + sprintController.SprintSpeed);
            else if (crouchController.IsCrouched) // Calculates HeadBob speed during crouched movement
                timer += Time.deltaTime * (BobSpeed * crouchController.SpeedReduction);
            else // Calculates HeadBob speed during walking
                timer += Time.deltaTime * BobSpeed;

            Joint.localPosition = new Vector3(
                jointOriginalPos.x + Mathf.Cos(timer) * bobAmount.x, 
                jointOriginalPos.y + Mathf.Cos(timer) * bobAmount.y, 
                jointOriginalPos.z + Mathf.Cos(timer) * bobAmount.z);
        }
        else // Resets when player stops moving
        {
            timer = 0;
            Joint.localPosition = new Vector3(
                    Mathf.Lerp(Joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * BobSpeed), 
                    Mathf.Lerp(Joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * BobSpeed), 
                    Mathf.Lerp(Joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * BobSpeed)
                );
        }
    }

    public void UpdateHeadBobState()
    {
        if (EnableHeadBob)
            HeadBob();
    }
}

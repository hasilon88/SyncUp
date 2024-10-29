using UnityEngine;

public class HeadBobController : MonoBehaviour
{

    public bool EnableHeadBob = true;
    public Transform Joint;
    public float BobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);
    private Vector3 jointOriginalPos;
    private float timer = 0;
    public FirstPersonController FirstPersonController;

    private void Awake()
    {
        jointOriginalPos = Joint.localPosition;
    }

    private void HeadBob()
    {
        if (FirstPersonController.isWalking)
        {
            if (FirstPersonController.isSprinting) // Calculates HeadBob speed during sprint
                timer += Time.deltaTime * (BobSpeed + FirstPersonController.sprintSpeed);
            else if (FirstPersonController.CrouchController.IsCrouched) // Calculates HeadBob speed during crouched movement
                timer += Time.deltaTime * (BobSpeed * FirstPersonController.CrouchController.SpeedReduction);
            else // Calculates HeadBob speed during walking
                timer += Time.deltaTime * BobSpeed;

            Joint.localPosition = new Vector3(
                jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, 
                jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, 
                jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
        }
        else // Resets when player stops moving
        {
            timer = 0;
            Joint.localPosition = 
                new Vector3(
                    Mathf.Lerp(Joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * BobSpeed), 
                    Mathf.Lerp(Joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * BobSpeed), 
                    Mathf.Lerp(Joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * BobSpeed));
        }
    }
    private void Update()
    {
        if (EnableHeadBob)
            HeadBob();
    }
}

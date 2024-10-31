using UnityEngine;

public class CrouchController : MonoBehaviour
{

    public bool EnableCrouch = true;
    public bool HoldToCrouch = true;
    public KeyCode CrouchKey = KeyCode.LeftControl;
    public float CrouchHeight = .75f;
    public float SpeedReduction = .5f;
    public bool IsCrouched = false;
    private Vector3 OriginalScale;
    private SprintController sprintController;

    private void Awake()
    {
        OriginalScale = transform.localScale;
    }

    private void Start()
    {
        sprintController = GetComponent<SprintController>();
    }

    public void Crouch()
    {
        if (IsCrouched) // Stands player up to full height // Brings walkSpeed back up to original speed
        {
            transform.localScale = new Vector3(OriginalScale.x, OriginalScale.y, OriginalScale.z);
            sprintController.WalkSpeed /= SpeedReduction;
            IsCrouched = false;
        }
        else // Crouches player down to set height // Reduces walkSpeed
        {
            transform.localScale = new Vector3(OriginalScale.x, CrouchHeight, OriginalScale.z);
            sprintController.WalkSpeed *= SpeedReduction;
            IsCrouched = true;
        }
    }

    public void UpdateCrouchState()
    {
        if (EnableCrouch)
        {
            if (Input.GetKeyDown(CrouchKey) && !HoldToCrouch)
                Crouch();
            if (Input.GetKeyDown(CrouchKey) && HoldToCrouch)
            {
                IsCrouched = false;
                Crouch();
            }
            else if (Input.GetKeyUp(CrouchKey) && HoldToCrouch)
            {
                IsCrouched = true;
                Crouch();
            }
        }
    }
    
}

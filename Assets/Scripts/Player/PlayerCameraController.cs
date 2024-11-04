using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCameraController : MonoBehaviour
{
    public Camera playerCamera;

    public float Fov = 60f;
    public bool InvertCamera = false;
    public bool CameraCanMove = true;
    public float MouseSensitivity = 2f;
    public float MaxLookAngle = 50f;

    public bool LockCursor = true;
    public bool Crosshair = true;
    public Sprite CrosshairImage;
    public Color CrosshairColor = Color.white;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;

    public bool EnableZoom = true;
    public bool HoldToZoom = false;
    public KeyCode ZoomKey = KeyCode.Mouse1;
    public float ZoomFOV = 30f;
    public float ZoomStepTime = 5f;

    public bool isZoomed = false;

    private SprintController sprintController;

    public void Awake()
    {
        sprintController = GetComponent<SprintController>();
        crosshairObject = GetComponentInChildren<Image>();
        playerCamera.fieldOfView = Fov;
    }

    public void Start()
    {
        SetUpCursor();
    }

    private void SetUpCursor()
    {
        if (LockCursor) Cursor.lockState = CursorLockMode.Locked;
        if (Crosshair)
        {
            crosshairObject.sprite = CrosshairImage;
            crosshairObject.color = CrosshairColor;
        }
        else crosshairObject.gameObject.SetActive(false);
    }

    public void UpdateZoomState()
    {
        if (EnableZoom)
        {
            if (Input.GetKeyDown(ZoomKey) && !HoldToZoom && !sprintController.IsSprinting)
            {
                if (!isZoomed) isZoomed = true;
                else isZoomed = false;
            }

            if (HoldToZoom && !sprintController.IsSprinting)
            {
                if (Input.GetKeyDown(ZoomKey)) isZoomed = true;
                else if (Input.GetKeyUp(ZoomKey)) isZoomed = false;
            }

            if (isZoomed)
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, ZoomFOV, ZoomStepTime * Time.deltaTime);
            else if (!isZoomed && !sprintController.IsSprinting)
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, Fov, ZoomStepTime * Time.deltaTime);
        }
    }

    public void UpdatePlayerCameraState()
    {
        if (CameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * MouseSensitivity;

            if (!InvertCamera)
                pitch -= MouseSensitivity * Input.GetAxis("Mouse Y");
            else
                pitch += MouseSensitivity * Input.GetAxis("Mouse Y"); // Inverted Y

            pitch = Mathf.Clamp(pitch, -MaxLookAngle, MaxLookAngle); // Clamp pitch between lookAngle

            transform.localEulerAngles = new Vector3(pitch, yaw, 0);
            //playerCamera.transform.localEulerAngles = new Vector3(pitch, yaw, 0); //to move head????
        }
    }

}

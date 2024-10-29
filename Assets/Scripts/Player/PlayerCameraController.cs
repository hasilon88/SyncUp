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

    // Crosshair
    public bool LockCursor = true;
    public bool Crosshair = true;
    public Sprite CrosshairImage;
    public Color CrosshairColor = Color.white;

    // Internal Variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;

    public bool EnableZoom = true;
    public bool HoldToZoom = false;
    public KeyCode ZoomKey = KeyCode.Mouse1;
    public float ZoomFOV = 30f;
    public float ZoomStepTime = 5f;

    public bool isZoomed = false;

    public FirstPersonController FirstPersonController;

    public void Awake()
    {
        crosshairObject = GetComponentInChildren<Image>();
        playerCamera.fieldOfView = Fov;
    }

    public void Start()
    {
        if (LockCursor) Cursor.lockState = CursorLockMode.Locked;

        if (Crosshair)
        {
            crosshairObject.sprite = CrosshairImage;
            crosshairObject.color = CrosshairColor;
        }
        else crosshairObject.gameObject.SetActive(false);
    }

    public void LateUpdate()
    {
        if (CameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * MouseSensitivity;

            if (!InvertCamera)
                pitch -= MouseSensitivity * Input.GetAxis("Mouse Y");
            else
                pitch += MouseSensitivity * Input.GetAxis("Mouse Y"); // Inverted Y

            pitch = Mathf.Clamp(pitch, -MaxLookAngle, MaxLookAngle); // Clamp pitch between lookAngle

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }

        if (EnableZoom)
        {
            // Changes isZoomed when key is pressed
            // Behavior for toogle zoom
            if (Input.GetKeyDown(ZoomKey) && !HoldToZoom && !FirstPersonController.isSprinting)
            {
                if (!isZoomed)
                    isZoomed = true;
                else
                    isZoomed = false;
            }

            // Changes isZoomed when key is pressed
            // Behavior for hold to zoom
            if (HoldToZoom && !FirstPersonController.isSprinting)
            {
                if (Input.GetKeyDown(ZoomKey))
                    isZoomed = true;
                else if (Input.GetKeyUp(ZoomKey))
                    isZoomed = false;
            }

            // Lerps camera.fieldOfView to allow for a smooth transistion
            if (isZoomed)
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, ZoomFOV, ZoomStepTime * Time.deltaTime);
            else if (!isZoomed && !FirstPersonController.isSprinting)
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, Fov, ZoomStepTime * Time.deltaTime);
        }

    }


}

using System;
using System.Collections;
using UnityEngine;

public class DashController : MonoBehaviour
{

    //public float DashSpeed = 5f;
    //public int DashDurationInFrames = 30;
    //public float DashCooldown = 1f;
    //public bool CurrentlyDashing = false;
    //public bool ShowDebugDashDirectionLine = false;
    //public event EventHandler OnDashIteration;
    //public event EventHandler OnDashStop;

    public enum DashTypes
    {
        RESTRICTED,
        OMNI_DIRECTIONAL
    }

    public float DashForce = 15f;
    public bool CanDash = true;
    public bool OnCooldown = false;
    public event EventHandler OnDashStart;
    public KeyCode TriggerKey;
    public DashTypes DashType;
    private PlayerController firstPersonController;
    private Vector3 playerVelocity;
    private GlobalStates globalStates = GlobalStates.Instance;

    private void Start()
    {
        if (TriggerKey == KeyCode.None) TriggerKey = KeyCode.F;
        firstPersonController = GetComponent<PlayerController>();
        OnDashStart += (object sender, EventArgs e) =>
        {
            Debug.Log("Dash...");
            playerVelocity = firstPersonController._rigidBody.velocity;
        };
    }

    public void UpdateDashState()
    {
        if (Input.GetKeyDown(TriggerKey) && CanDash && !OnCooldown && globalStates.DashIsUnlocked)
        {
            OnDashStart?.Invoke(this, EventArgs.Empty);
            switch (DashType)
            {
                case DashTypes.RESTRICTED:
                    firstPersonController._rigidBody.AddForce(new Vector3(playerVelocity.x, 0, playerVelocity.z) * DashForce, ForceMode.VelocityChange);
                    break;
                case DashTypes.OMNI_DIRECTIONAL:
                    firstPersonController._rigidBody.AddForce(playerVelocity * DashForce, ForceMode.VelocityChange);
                    break;
            }
        }
    }

}

using UnityEngine;

public class DashController : MonoBehaviour
{

    public float DashForce = 5f;
    public float DashCooldown = 1f;
    public bool OnCooldown = false;
    public bool ShowDebugDashDirectionLine = false;
    public KeyCode TriggerKey;

    private void Start()
    {
        
    }
}

using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    public Rigidbody rb;
    public bool PlayerCanMove = true;

    public PlayerCameraController PlayerCameraController;
    public JumpController JumpController;
    public CrouchController CrouchController;
    public SprintController SprintController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

}
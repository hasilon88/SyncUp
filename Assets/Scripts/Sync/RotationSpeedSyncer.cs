using UnityEngine;

public class RotationSpeedSyncer : MonoBehaviour
{

    public RotationSpeedSync RotationSpeedSync;
    [Range(500f, 8000f)]
    public float InitialSpeed = 1000f;

    void Update()
    {
        this.transform.Rotate(Vector3.up, 1000f * this.RotationSpeedSync.RotationSpeed * Time.deltaTime, Space.World);
    }
}

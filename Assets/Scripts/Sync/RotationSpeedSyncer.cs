using UnityEngine;

public class RotationSpeedSyncer : MonoBehaviour
{

    public RotationSpeedSync RotationSpeedSync;
    [Range(1f, 5f)]
    public float InitialSpeed = 1f;

    void Update()
    {
        //InitialSpeed * this.RotationSpeedSync.RotationSpeed * Time.deltaTime, Space.World
        //InitialSpeed * this.RotationSpeedSync.RotationSpeed * (1f + Time.deltaTime)
        this.transform.Rotate(Vector3.up, 850f * this.RotationSpeedSync.RotationSpeed * Time.deltaTime, Space.World);
    }
}

using UnityEngine;

public class RotationSpeedSyncer : MonoBehaviour
{

    public RotationSpeedSync RotationSpeedSync;
    [Range(500f, 8000f)]
    public float InitialSpeed = 1000f;

    void Update()
    {
        //InitialSpeed * this.RotationSpeedSync.RotationSpeed * Time.deltaTime, Space.World
        //InitialSpeed * this.RotationSpeedSync.RotationSpeed * (1f + Time.deltaTime)
        this.transform.Rotate(Vector3.up, InitialSpeed * this.RotationSpeedSync.RotationSpeed * Time.deltaTime, Space.World);
    }
}

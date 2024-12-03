using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSpin : MonoBehaviour
{
    public Camera Camera;
    public float Speed = 5f;

    void Update()
    {
        transform.RotateAround(Camera.transform.position, Vector3.up, Speed * Time.deltaTime);
    }
}

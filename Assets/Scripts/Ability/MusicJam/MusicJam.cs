using CSCore.Win32;
using System.Collections.Generic;
using UnityEngine;

public class MusicJam : MonoBehaviour
{

    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public float throwCoolDown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.F;
    public float throwForce;
    public float throwUpwardForce;

    private bool readyToThrow;

    void Start()
    {
        readyToThrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(throwKey) && readyToThrow)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        Rigidbody body = projectile.GetComponent<Rigidbody>();

        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;

        body.AddForce(forceToAdd, ForceMode.Impulse);

        Invoke(nameof(ResetThrow), throwCoolDown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDash : MonoBehaviour
{

    public PlayerController Controller;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Controller.EnableDashState();
        }
    }
}

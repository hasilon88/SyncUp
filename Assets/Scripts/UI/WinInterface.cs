using CSCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinInterface : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameObject.Find("WinOverlay").SetActive(true);
        }
    }
}

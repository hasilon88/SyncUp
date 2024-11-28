using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinEnd : MonoBehaviour
{

    private PauseController pauseController;

    private void Start()
    {
        pauseController = GameObject.Find("PauseController").GetComponent<PauseController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pauseController.ActuateWinOverlay();
        }
    }



}
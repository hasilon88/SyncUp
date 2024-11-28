using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathInterfaceBehavior : MonoBehaviour
{

    public Button QuitButton;
    public Button RestartButton;
    private PauseController pauseController;

    void Start()
    {
        RestartButton.onClick.AddListener(() =>
        {
            var sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        });
        pauseController = GameObject.Find("PauseController").GetComponent<PauseController>();
        QuitButton.onClick.AddListener(Exit);
    }

    private void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

}

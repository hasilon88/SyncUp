using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseInterfaceBehavior : MonoBehaviour
{

    private PauseController pauseController;
    private Button resumeButton;
    private Button titleScreenButton;
    private Button quitButton;
    private Button restartButton;

    void Start()
    {
        pauseController = GameObject.FindGameObjectWithTag("PauseController").GetComponent<PauseController>();
        resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        titleScreenButton = GameObject.Find("TitleScreenButton").GetComponent<Button>();
        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        restartButton = ComponentUtils.Find<Button>("RestartButton");
        restartButton.onClick.AddListener(Restart);
        resumeButton.onClick.AddListener(pauseController.UnPause);
        titleScreenButton.onClick.AddListener(GoBackToTitleScreen);
        quitButton.onClick.AddListener(UIUtils.Exit);
    }

    public void GoBackToTitleScreen()
    {
        OverlayController.PrepareNavigation();
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        OverlayController.PrepareNavigation();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
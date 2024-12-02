using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseInterfaceBehavior : MonoBehaviour
{

    private PauseController pauseController;
    private Button resumeButton;
    //private Button titleScreenButton;
    //private Button quitButton;
    //private Button restartButton;

    void Start()
    {
        pauseController = GameObject.FindGameObjectWithTag("PauseController").GetComponent<PauseController>();
        resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        resumeButton.onClick.AddListener(pauseController.UnPause);
        //quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        //titleScreenButton = GameObject.Find("TitleScreenButton").GetComponent<Button>();
        //restartButton = ComponentUtils.Find<Button>("RestartButton");
        //restartButton.onClick.AddListener(Restart);
        //titleScreenButton.onClick.AddListener(GoBackToTitleScreen);
        //quitButton.onClick.AddListener(UIUtils.Exit);
    }

}
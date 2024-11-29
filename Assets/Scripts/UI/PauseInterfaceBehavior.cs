using UnityEngine.UI;
using UnityEngine;

public class PauseInterfaceBehavior : MonoBehaviour
{

    private PauseController pauseController;
    private Button resumeButton;
    private Button toMenuButton;
    private Button quitButton;

    void Start()
    {
        pauseController = GameObject.FindGameObjectWithTag("PauseController").GetComponent<PauseController>();
        resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        toMenuButton = GameObject.Find("ToMenuButton").GetComponent<Button>();
        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        resumeButton.onClick.AddListener(pauseController.UnPause);
        toMenuButton.onClick.AddListener(() => { Debug.Log("to menu..."); } );
        quitButton.onClick.AddListener(UIUtils.Exit);

    }

}

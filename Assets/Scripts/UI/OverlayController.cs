using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public enum OverlayType
{
    GAME,
    PAUSE,
    DEATH,
    COMPLETION
}

public class OverlayController : MonoBehaviour
{

    private Canvas gameOverlay;
    private Canvas pauseOverlay;
    private Canvas deathOverlay;
    private Canvas completionOverlay;

    private void Start()
    {
        gameOverlay = ComponentUtils.Find<Canvas>("GameOverlay");
        pauseOverlay = ComponentUtils.Find<Canvas>("PauseOverlay");
        deathOverlay = ComponentUtils.Find<Canvas>("DeathOverlay");
        completionOverlay = ComponentUtils.Find<Canvas>("CompletionOverlay");
        ChangeOverlay(OverlayType.GAME);
        SetBasicButtonsCallbacks();
    }

    public void ChangeOverlay(OverlayType type)
    {
        switch (type)
        {
            case OverlayType.GAME:
                ChangeOverlayState(gameOverlayS: true);
                break;
            case OverlayType.PAUSE:
                ChangeOverlayState(pauseOverlayS: true);
                break;
            case OverlayType.DEATH:
                ChangeOverlayState(deathOverlayS: true);
                break;
            case OverlayType.COMPLETION:
                ChangeOverlayState(completionOverlayS: true);
                break;
        }
    }

    public void SetBasicButtonsCallbacks()
    {
        Button[] pauseOverlayButtons = pauseOverlay.GetComponentsInChildren<Button>();
        Button[] deathOverlayButtons = deathOverlay.GetComponentsInChildren<Button>();
        Button[] completionOverlayButtons = completionOverlay.GetComponentsInChildren<Button>();
        Button[] buttons = pauseOverlayButtons.Concat(deathOverlayButtons).Concat(completionOverlayButtons).ToArray();
        foreach (Button button in buttons) 
        {
            switch(button.gameObject.name)
            {
                case "QuitButton":
                    button.onClick.AddListener(UIUtils.Exit);
                    break;
                case "RestartButton":
                    button.onClick.AddListener(Restart);
                    break;
                case "TitleScreenButton":
                    button.onClick.AddListener(GoBackToTitleScreen);
                    break;
            }        
        }
    }

    public void GoBackToTitleScreen()
    {
        PrepareNavigation();
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        PrepareNavigation();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ChangeOverlayState(bool gameOverlayS = false, bool pauseOverlayS = false, bool deathOverlayS = false, bool completionOverlayS = false)
    {
        gameOverlay.gameObject.SetActive(gameOverlayS);
        pauseOverlay.gameObject.SetActive(pauseOverlayS);
        deathOverlay.gameObject.SetActive(deathOverlayS);
        completionOverlay.gameObject.SetActive(completionOverlayS);
    }

    public static void PrepareNavigation()
    {
        GameEnvironment.Instance = null;
        AudioManager.Instance.StartCapture();
        Time.timeScale = 1f;
    }

}


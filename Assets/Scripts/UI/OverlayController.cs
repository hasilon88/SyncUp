using UnityEngine;

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
        Debug.Log(gameOverlay != null);
        pauseOverlay = ComponentUtils.Find<Canvas>("PauseOverlay");
        deathOverlay = ComponentUtils.Find<Canvas>("DeathOverlay");
        completionOverlay = ComponentUtils.Find<Canvas>("CompletionOverlay");
        ChangeOverlay(OverlayType.GAME);
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


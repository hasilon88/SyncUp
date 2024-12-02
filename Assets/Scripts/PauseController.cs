using System;
using UnityEngine;

public class PauseController : MonoBehaviour
{

    public bool GameIsPaused = false;
    public KeyCode PauseKey = KeyCode.None;
    private PlayerController playerController;
    private AudioManager audioManager;
    private OverlayController overlayController;

    public event EventHandler OnPauseEnter;
    public event EventHandler OnPauseLeave;

    private void Start()
    {
        if (PauseKey == KeyCode.None) PauseKey = KeyCode.Escape;
        audioManager = AudioManager.Instance;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        overlayController = ComponentUtils.Find<Canvas>("Overlays").GetComponent<OverlayController>();

        OnPauseEnter += (object sender, EventArgs e) =>
        {
            playerController.PlayerCanMove = false;
            GameIsPaused = true;
            Cursor.lockState = CursorLockMode.None;
            overlayController.ChangeOverlay(OverlayType.PAUSE);
            audioManager.StopCapture();
        };

        OnPauseLeave += (object sender, EventArgs e) =>
        {
            playerController.PlayerCanMove = true;
            GameIsPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            overlayController.ChangeOverlay(OverlayType.GAME);
            audioManager.StartCapture();
        };
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        OnPauseEnter?.Invoke(this, EventArgs.Empty);
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        OnPauseLeave?.Invoke(this, EventArgs.Empty);
    }

    public void TogglePause()
    {
        if (GameIsPaused)
            UnPause();
        else
            Pause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(PauseKey))
            TogglePause();
    }
}

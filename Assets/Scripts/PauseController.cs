using System;
using UnityEngine;

public class PauseController : MonoBehaviour
{

    public PauseController Instance;
    public bool GameIsPaused = false;
    public KeyCode PauseKey;
    private PlayerController firstPersonController;
    public event EventHandler OnPauseEnter;
    public event EventHandler OnPauseLeave;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(this);
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        PauseKey = KeyCode.Escape;
        firstPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        OnPauseEnter += (object sender, EventArgs e) => 
        {
            firstPersonController.PlayerCanMove = false;
            GameIsPaused = true;
            Cursor.lockState = CursorLockMode.None;
        };

        OnPauseLeave += (object sender, EventArgs e) =>
        {
            firstPersonController.PlayerCanMove = true;
            GameIsPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
        };
    }

    public void Pause() 
    {
        Time.timeScale = 0;
        OnPauseEnter?.Invoke(this, EventArgs.Empty);
    }

    public void UnPause()
    {
        Time.timeScale = 1;
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

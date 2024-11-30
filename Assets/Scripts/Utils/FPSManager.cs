using UnityEngine;

/// <summary>
/// QualitySettings.vSyncCount = this.VSyncLevel;
/// Application.targetFrameRate = this.FPSLock;
/// Source: https://discussions.unity.com/t/how-do-i-find-the-frames-per-second-of-my-game/14717/2
/// FixedUpdateFrameCount?
/// LateUpdateFrameCount?
/// </summary>
public class FPSManager : MonoBehaviour
{

    public static FPSManager Instance;
    public byte VSyncLevel = 0;
    public float FrameRate = 0f;
    public int FPSLock = 60;
    public int UpdateFrameCount = 0; //remove?
    public bool Locked = true; //remove?

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this); 
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (Locked)
        {
            QualitySettings.vSyncCount = this.VSyncLevel;
            Application.targetFrameRate = this.FPSLock;
        }
    }

    void Update()
    {
        this.FrameRate = (1.0f / Time.smoothDeltaTime);
        this.UpdateFrameCount++;
        if (this.UpdateFrameCount >= 10000) this.UpdateFrameCount = 0;
    }
}

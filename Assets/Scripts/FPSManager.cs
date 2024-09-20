using UnityEngine;

public class FPSManager : MonoBehaviour
{

    public byte VSyncLevel = 0;
    public float TotalDeltaTimeSinceSessionStart = 0;
    public float TempDeltaTotalTime = 0;
    public float FrameRate = 0;
    public int FrameCount = 0;
    public int FPSLock = 59;
    
    void Start()
    {
        QualitySettings.vSyncCount = this.VSyncLevel;
        Application.targetFrameRate = this.FPSLock;
    }

    //Source: https://discussions.unity.com/t/how-do-i-find-the-frames-per-second-of-my-game/14717/2
    void Update()
    {
        this.TotalDeltaTimeSinceSessionStart += Time.deltaTime;
        this.TempDeltaTotalTime += Time.deltaTime;
        this.FrameCount++;
        this.FrameRate = (1.0f / Time.smoothDeltaTime);
        if (this.TempDeltaTotalTime > 1)
        {
            this.FrameCount = 0;
            this.TempDeltaTotalTime = 0;
        }
    }
}

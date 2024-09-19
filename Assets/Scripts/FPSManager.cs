using UnityEngine;

public class FPSManager : MonoBehaviour
{

    public byte VSyncLevel = 0;
    public float TotalTimeSinceSessionStart = 0;
    public float TempTotalTime = 0;
    public float FrameRate = 0;
    public int FrameCount = 0;
    public int FPSLock = 59;
    
    void Start()
    {
        QualitySettings.vSyncCount = this.VSyncLevel;
        Application.targetFrameRate = this.FPSLock;
    }

    private void AlternateUpdate()
    {

    }

    //Source: https://discussions.unity.com/t/how-do-i-find-the-frames-per-second-of-my-game/14717/2
    void Update()
    {
        this.TotalTimeSinceSessionStart += Time.deltaTime;
        this.TempTotalTime += Time.deltaTime;
        this.FrameCount++;
        if (this.TempTotalTime > 1)
        {
            this.FrameRate = this.FrameCount;
            this.FrameCount = 0;
            this.TempTotalTime = 0;
        }
        Debug.Log((int)(1.0f / Time.smoothDeltaTime));
    }
}

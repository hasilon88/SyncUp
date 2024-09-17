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
    }
}

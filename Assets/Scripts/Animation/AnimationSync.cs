using UnityEngine;

public class AnimationSync : SyncObject
{

    public float AnimationSpeed;

    void Update()
    {
        if (this.FPSManager.FrameCount % this.AudioManager.LastSamplesTempo == 0)
        {
            this.AnimationSpeed = 1.0f + this.AudioManager.CurrentToMaxFrequencyPercentage/100f;
        }
    }
}

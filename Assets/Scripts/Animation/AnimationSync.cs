using UnityEngine;

public class AnimationSync: SyncObject
{

    public float AnimationSpeed;
    public float InitialSpeed = 1.0f;

    public override void Sync()
    {
        if (this.AudioManager.FPSManager.UpdateFrameCount % this.AudioManager.LastLoudestSamplesFrameTempo == 0)
        {
            this.AnimationSpeed = this.InitialSpeed + this.AudioManager.NormalizedCurrentLoudestSample_LastLoudestSamplesMax;
        }
    }
}

using UnityEngine;

public class AnimationSync: SyncObject
{

    public float AnimationSpeed;
    public float InitialSpeed = 1.0f;

    public override void Sync()
    {
        this.AnimationSpeed = this.InitialSpeed + this.AudioManager.NormalizedCurrentLoudestSample_LastLoudestSamplesMax;
    }
}

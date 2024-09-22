using UnityEngine;

//CHARACTER ANIMATIONS SHOULD BE HANDLED BY THE MUSIC
public class AnimationSync : SyncObject
{

    public float AnimationSpeed;

    void Update()
    {
        if (this.FPSManager.FrameCount % this.FrameTempo == 0)
            this.AnimationSpeed = 1.0f + this.AudioManager.CurrentToMaxFrequencyPercentage / 100f;
    }
}

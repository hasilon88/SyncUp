using UnityEngine;

public class AnimationSync : SyncObject
{

    public float AnimationSpeed;

    void Update()
    {
        this.OnFrameTempo(() =>
        {
            this.AnimationSpeed = 1.0f + this.AudioManager.CurrentToMaxFrequencyPercentage / 100f;
        });
            
    }
}

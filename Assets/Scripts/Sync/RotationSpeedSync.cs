using UnityEngine;

public class RotationSpeedSync : SyncObject
{

    public float RotationSpeed = 0f;

    public override void Sync()
    {
        this.RotationSpeed = this.AudioManager.NormalizedCurrentLoudestSample_LastLoudestSamplesMax;
    }

}

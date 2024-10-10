using System;
using UnityEngine;

public abstract class SyncObject : MonoBehaviour
{

    public AudioManager AudioManager;

    private void Start()
    {
        this.AudioManager.OnNormalizedValuesChange += SyncOnLastLoudestSemplesFrameTempo;
    }

    public void SyncOnLastLoudestSemplesFrameTempo(object sender, EventArgs e)
    {
        if (this.AudioManager.FPSManager.UpdateFrameCount % this.AudioManager.LastLoudestSamplesFrameTempo == 0)
            Sync();
    }

    public abstract void Sync();
}

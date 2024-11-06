using System;
using UnityEngine;

public abstract class SyncObject : MonoBehaviour
{
    public enum OptionalTempo
    {
        ON_CAPTURE_RATE,
        ON_FRAME_TEMPO
    }

    protected AudioManager AudioManager;
    public OptionalTempo OptionalTempo_ = OptionalTempo.ON_CAPTURE_RATE;

    private void Start()
    {
        if (this.AudioManager == null) 
            this.AudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        if (this.AudioManager != null && this.OptionalTempo_ == OptionalTempo.ON_CAPTURE_RATE)
            this.AudioManager.OnNormalizedValuesChange += SyncOnLastLoudestSamplesFrameTempo;
        else if (this.AudioManager != null && this.OptionalTempo_ == OptionalTempo.ON_FRAME_TEMPO)
            this.AudioManager.OnNormalizedValuesChange += SyncOnLastLoudestSamplesFrameTempo;
    }

    public void SyncOnCaptureRate(object sender, EventArgs e)
    {
        Sync();
    }

    public void SyncOnLastLoudestSamplesFrameTempo(object sender, EventArgs e)
    {
        if (this.AudioManager.FPSManager.UpdateFrameCount % this.AudioManager.LastLoudestSamplesFrameTempo == 0)
            Sync();
    }

    public abstract void Sync();
}

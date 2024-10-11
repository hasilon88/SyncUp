using System;
using UnityEngine;

public abstract class SyncObject : MonoBehaviour
{

    protected AudioManager AudioManager;

    private void Start()
    {
        if (this.AudioManager == null) 
            this.AudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        if (this.AudioManager != null) 
            this.AudioManager.OnNormalizedValuesChange += SyncOnLastLoudestSemplesFrameTempo;
    }

    public void SyncOnLastLoudestSemplesFrameTempo(object sender, EventArgs e)
    {
        if (this.AudioManager.FPSManager.UpdateFrameCount % this.AudioManager.LastLoudestSamplesFrameTempo == 0)
            Sync();
    }

    public abstract void Sync();
}

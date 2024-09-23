using UnityEngine;

public class SyncObject : MonoBehaviour
{
    [Range(0, 32)]
    public int FrameTempo = 4;
    public AudioManager AudioManager;
    public FPSManager FPSManager;
    protected delegate void Callback();

    protected void OnLastSamplesTempo(Callback cb)
    {
        if (this.FPSManager.FrameCount % this.AudioManager.LastSamplesTempo == 0) cb();
    }

    protected void OnFrameTempo(Callback cb)
    {
        if (this.FPSManager.FrameCount % this.FrameTempo == 0) cb();
    }

}

using UnityEngine;

public class SyncObject : MonoBehaviour
{
    [Range(0, 32)]
    public int FrameTempo = 4;
    public AudioManager AudioManager;
    public FPSManager FPSManager;
}

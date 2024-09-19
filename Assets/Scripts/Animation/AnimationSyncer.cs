using UnityEngine;

public class AnimationSyncer : MonoBehaviour
{

    public Animator Animator;
    public AudioManager AudioManager;
    public float AnimationSpeed;
    public FPSManager FPSManager;
    [Range(0, 32)]
    public int Tempo = 4;


    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (this.FPSManager.FrameCount % this.Tempo == 0) {
            this.AnimationSpeed = 1.0f + (AudioManager.CurrentMaxFrequency/AudioManager.AverageMaxSampleFrequency);
            Animator.speed = this.AnimationSpeed;
        }
    }
}

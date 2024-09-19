using UnityEngine;

public class AnimationSyncer : MonoBehaviour
{

    public Animator Animator;
    public AudioManager AudioManager;

    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        Animator.speed = 1.0f + AudioManager.CurrentMaxFrequency;
    }
}

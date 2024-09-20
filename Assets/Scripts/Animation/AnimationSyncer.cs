using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSyncer : MonoBehaviour
{

    private Animator animator;
    public AnimationSync AnimationSync;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.speed = AnimationSync.AnimationSpeed;
    }
}

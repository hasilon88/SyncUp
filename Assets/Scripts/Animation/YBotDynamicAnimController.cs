using UnityEngine;


//https://discussions.unity.com/t/find-all-animator-transitions/666496/5 GetTransition
public class YBotDynamicAnimController : MonoBehaviour
{

    private Animator animator;
    public AudioManager AudioManager;
    public FPSManager FPSManager;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (this.FPSManager.UpdateFrameCount % this.FPSManager.FPSLock == 0)
        {
            this.animator.SetFloat("frequencyPercentage", this.AudioManager.NormalizedCurrentLoudestSample_LastLoudestSamplesMax*100);
        }
    }
}

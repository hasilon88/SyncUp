using UnityEngine;

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
        this.animator.SetFloat("frequencyPercentage", this.AudioManager.CurrentToMaxFrequencyPercentage);
    }
}

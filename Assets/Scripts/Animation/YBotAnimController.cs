using UnityEngine;

public class YBotAnimController : MonoBehaviour
{

    private Animator Animator;

    void Start()
    {
        Animator = GetComponent<Animator>();    
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) Animator.SetBool("IsRunning", true);
        else Animator.SetBool("IsRunning", false);
    }
}

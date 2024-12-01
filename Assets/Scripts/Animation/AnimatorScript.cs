using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
    private SprintController sprintcontroller;
    private Animator animator;
    private GameObject player;
    public bool isEmptyGun = false;
    public bool isShooting = false;
    public bool isJumping = false;
    public bool isComing = false;
    public bool isSprinting = false;
    public bool isReload = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     player = GameObject.FindWithTag("Arms");
     sprintcontroller  = GetComponent<SprintController>();
     animator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SetWalkAnimation(sprintcontroller.IsWalking);
        SetJumpAnimation(isJumping);
        SetShootAnimation(isShooting);
        SetReloadAnimation(isReload);
        SetEmptyGunAnimation(isEmptyGun);
        SetComeAnimation(isComing);
        SetJogAnimation(isSprinting);

    }
    private void SetWalkAnimation(bool isWalk)
    {
        if (isWalk)
        {
            animator.SetTrigger("walk");
        }
        else
        {
            animator.ResetTrigger("walk");
            animator.SetTrigger("idle");
        }
    }
    private void SetShootAnimation(bool isShooting)
    {
        if (isShooting)
        {
            animator.SetTrigger("shoot");
        }
        else
        {
            animator.ResetTrigger("shoot");
            animator.SetTrigger("idle");
        }
    }
    private void SetJumpAnimation(bool isJumping)
    {
        if (isJumping)
        {
            animator.SetTrigger("jump");
        }
        else
        {
            animator.ResetTrigger("jump");
            animator.SetTrigger("idle");
        }
    }
    private void SetReloadAnimation(bool isReloading)
    {
        if (isReloading)
        {
            animator.SetTrigger("reload");
        }
        else
        {
            animator.ResetTrigger("reload");
            animator.SetTrigger("idle");
        }
    }
    private void SetEmptyGunAnimation(bool isEmptyGun)
    {
        if (isEmptyGun)
        {
            animator.ResetTrigger("shoot");
            animator.SetTrigger("emptyGun");
        }
        else
        {
            animator.ResetTrigger("emptyGun");
            animator.SetTrigger("idle");
        }
    }
    private void SetComeAnimation(bool isComing)
    {
        if (isComing)
        {
            animator.SetTrigger("come");
        }
        else
        {
            animator.ResetTrigger("come");
            animator.SetTrigger("idle");
        }
    }
    private void SetJogAnimation(bool isJogging)
    {
        if (isJogging)
        {
            animator.SetTrigger("jog");
        }
        else
        {
            animator.ResetTrigger("jog");
            animator.SetTrigger("idle");
        }
    }


}

using UnityEngine;

public class DeathOnCollision : MonoBehaviour
{

    private PlayerController playerController;

    private void Start()
    {
        playerController = ComponentUtils.FindFirstWithTag<PlayerController>("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.Die();
        }
    }
}

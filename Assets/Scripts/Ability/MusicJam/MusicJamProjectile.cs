
using UnityEngine;

public class MusicJamProjectile : MonoBehaviour
{

    bool bounced = false;

    // Gets called at the start of the collision 
    void OnCollisionEnter(Collision collision)
    {
        if (!bounced)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            body.velocity = new(0f, 3f, 0f);
            bounced = true;
            return;
        }
        if (bounced)
            Destroy(gameObject);

    }
}
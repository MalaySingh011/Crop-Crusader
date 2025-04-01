using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float pushForce = 20f;
    [SerializeField] private Rigidbody2D rb;

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, pushForce);
            anim.SetTrigger("activate");
        }
    }
}

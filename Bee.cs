using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class Bee : MonoBehaviour
{
    public Animator animator;
    public AIPath aiPath;
    public AIDestinationSetter aiDestinationSetter;

    public float health = 5f;
    public Slider healthBar;

    void Start()
    {
        aiDestinationSetter.enabled = false;
        healthBar.value = health;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            aiDestinationSetter.enabled = true;
            animator.SetBool("isFollowing", true);
            if (aiPath.desiredVelocity.x >= 0.01f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (aiPath.desiredVelocity.x <= -0.01f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        else
        {
            animator.SetBool("isFollowing", false);
            aiDestinationSetter.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool("isFollowing", false);
            aiDestinationSetter.enabled = false;
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool("isFollowing", false);
            animator.SetTrigger("Attack");
            animator.SetBool("isFollowing", true);
            StaminaHealth.instance.TakeDamage("Enemy");
        }
    }

    public void Kill()
    {
        health--;
        healthBar.value = health;
        StaminaHealth.instance.GainPoints();
        Snail.currkills++;
        Destroy(this.gameObject);
    }

    public void LowerHealth()
    {
        health--;
        healthBar.value = health;
        animator.SetTrigger("Hurt");
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Snail : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private float speed = 5f;
    public float health = 5f;
    public Slider healthBar;
    public static Snail instance;

    public static int totalkills = 0;
    public static int currkills = 0;

    void Start()
    {
        healthBar.value = health;
        instance = this;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            myRigidbody.linearVelocity = new Vector2(speed, myRigidbody.linearVelocity.y);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            myRigidbody.linearVelocity = new Vector2(speed, myRigidbody.linearVelocity.y);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            myRigidbody.linearVelocity = new Vector2(0, myRigidbody.linearVelocity.y);
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            speed = -speed;
            transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            speed = -speed;
            transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
        }
    }

    public void Die()
    {
        StaminaHealth.instance.GainPoints();
        anim.SetTrigger("Die");
        healthBar.value = 0;
        currkills++;
        Invoke("Destroy", .5f);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void LowerHealth()
    {
        health--;
        healthBar.value = health;
        anim.SetTrigger("Hurt");
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
}

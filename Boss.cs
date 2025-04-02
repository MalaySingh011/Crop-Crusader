using UnityEngine;
using UnityEngine.UI;

//Creates a Boss
public class Boss : MonoBehaviour
{
    public GameObject bullet;
    public Transform gun;
    public float spawntime;
    bool triggered = false;

    public GameObject waypoint1;
    public GameObject waypoint2;

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbody;
    public static Boss instance;

    int health = 15;
    public Slider healthBar;

    //Creates an instance of the boss and makes it start walking
    void Start()
    {
        instance = this;
        WalkNeg();
    }

    //Rounds values and triggers it to stop at endpoints
    void Update()
    {
        healthBar.value = health;
        float xpos = Mathf.Round(transform.position.x * 100.0f) * .01f;
        float xpos1 = Mathf.Round(waypoint1.transform.position.x * 100.0f) * .01f;
        float xpos2 = Mathf.Round(waypoint2.transform.position.x * 100.0f) * .01f;

        if (Mathf.Approximately(xpos, xpos1) && !triggered)
        {
            rigidbody.linearVelocity = new Vector2(0, 0);
            animator.SetBool("isWalking", false);
            Invoke("WalkNeg", 3f);
        }
        if (Mathf.Approximately(xpos, xpos2) && !triggered)
        {
            rigidbody.linearVelocity = new Vector2(0, 0);
            animator.SetBool("isWalking", false);
            Invoke("WalkPos", 3f);
        }
    }

    //Walks in the positive direciton
    void WalkPos()
    {
        animator.SetBool("isWalking", true);
        rigidbody.linearVelocity = new Vector2(1, rigidbody.linearVelocity.y);
        transform.localScale = new Vector3((Mathf.Sign(rigidbody.linearVelocity.x)), 1f, 1f);
        healthBar.transform.localScale = new Vector3(.05f, .1f, 1f);
    }

    //Walks in the negative direction
    void WalkNeg()
    {
        animator.SetBool("isWalking", true);
        rigidbody.linearVelocity = new Vector2(-1, rigidbody.linearVelocity.y);
        transform.localScale = new Vector3((Mathf.Sign(rigidbody.linearVelocity.x)), 1f, 1f);
        healthBar.transform.localScale = new Vector3(-.05f, .1f, 1f);
    }

    //Calls the boss to attack
    void Fire()
    {
        animator.SetBool("isWalking", false);
        animator.SetTrigger("Attack");
        Instantiate(bullet, gun.position, transform.rotation);
    }

//Repeatedly fires at the player for a random period of time and stops walking when the player enters the trigger box
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            CheckDirection();
            InvokeRepeating("Fire", spawntime, Random.Range(3f, 5f));
            rigidbody.linearVelocity = new Vector2(0, 0);
            triggered = true;
            CancelInvoke("WalkPos");
            CancelInvoke("WalkNeg");
        }
    }

    //Stops firing and walks when the player exits the trigger
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && other == other.GetComponent<CapsuleCollider2D>())
        {
            triggered = false;
            CancelInvoke("Fire");
            Invoke("WalkNeg", 1f);
        }
    }

    //Switches the direction the boss is looking at to face the player
    void CheckDirection()
    {
        if (transform.position.x < Player.instance.transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            healthBar.transform.localScale = new Vector3(.05f, .1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            healthBar.transform.localScale = new Vector3(-.05f, .1f, 1f);
        }
    }

    //Damages the boss and checks for death
    public void TakeDamage()
    {
        CancelInvoke("Fire");
        CancelInvoke("WalkPos");
        CancelInvoke("WalkNeg");
        
        if (health-1 <= 0)
        {
            animator.SetTrigger("Dead");
            health--;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            Invoke("Death", 2f);
        }
        else
        {
            health--;
            animator.SetTrigger("Hurt");
            CheckDirection();
            InvokeRepeating("Fire", spawntime, Random.Range(3f, 5f));
        }
    }

    //Kills the boss
    void Death()
    {
        Destroy(this.gameObject);
    }
}

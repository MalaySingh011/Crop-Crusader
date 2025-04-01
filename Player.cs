using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float runSpeed = 15f;
    [SerializeField] float jumpForce = 7.5f;

    private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    private float hold_counter = 0f;
    private float attack_counter = 0f;
    private float damage_counter = 0f;
    private bool isGrounded = true;
    public bool isAttacking = false;
    bool isDead = false;
    public static Player instance;

    [SerializeField] private Title title;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim.SetBool("isMoving", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isClimbing", false);
        instance = this;
    }

    void Update()
    {
        anim.SetInteger("y-velocity", Mathf.RoundToInt(rb.linearVelocity.y));
        if (StaminaHealth.instance.currentStamina <= 0)
        {
            StaminaHealth.instance.currentStamina = 0;
        }
        if (DialogueManager.isTalking)
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isClimbing", false);
            rb.linearVelocity = new Vector2(0, 0);
            return;
        }
        if (isDead)
        {
            StaminaHealth.instance.currentHealth = 0;
            rb.linearVelocity = new Vector2(0, 0);
            return;
        }
        if (Title.isPaused || DialogueManager.isTalking || Title.isShopped)
        {
            return;
        }
        else
        {
            float horizontal = Input.GetAxis("Horizontal");
            Walk();
            Jump();
            Run();
            //Attacking
            if (Input.GetKey(KeyCode.Mouse0) && StaminaHealth.instance.currentStamina-5 >= 0)
            {
                isAttacking = true;
                Attack();
            }
            else
            {
                isAttacking = false;
            }
            //Running
            if (Input.GetKey(KeyCode.LeftShift) && StaminaHealth.instance.currentStamina-5 > 0)
            {
                hold_counter += Time.deltaTime;
                rb.linearVelocity = new Vector2(horizontal * runSpeed, rb.linearVelocity.y);
                if (hold_counter >= 1f)
                {
                    anim.SetBool("isRunning", true);
                    StaminaHealth.instance.UseStamina();
                    hold_counter = 0f;
                }
            }
            else
            {
                hold_counter += Time.deltaTime;
                anim.SetBool("isRunning", false);
                rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
                if (hold_counter >= 1f)
                {
                    StaminaHealth.instance.GainStamina();
                    hold_counter = 0f;
                }
            }
            if (StaminaHealth.instance.currentHealth <= 0)
            {
                return;
            }
        }
    }

    private void Walk()
    {
        float horizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        if(horizontal > 0)
        {
            anim.SetBool("isMoving", true);
            this.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
        else if (horizontal < 0)
        {
            anim.SetBool("isMoving", true);
            this.transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
        if (StaminaHealth.instance.currentHealth <= 0)
        {
            return;
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
            StaminaHealth.instance.UseStamina();
            StaminaHealth.instance.UseStamina();
        }
        if (StaminaHealth.instance.currentHealth-5 <= 0)
        {
            return;
        }
    }

    private void Run()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Run") && StaminaHealth.instance.currentStamina > 0)
        {
            if(horizontal > 0)
            {
                anim.SetBool("isRunning", true);
                this.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            }
            else if (horizontal < 0)
            {
                anim.SetBool("isRunning", true);
                this.transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
            } 
            else
            {
                anim.SetBool("isRunning", false);
            }
        }
        if (Input.GetButtonUp("Run"))
        {
            anim.SetBool("isRunning", false);
        }
        if (StaminaHealth.instance.currentHealth <= 0)
        {
            return;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
        if (StaminaHealth.instance.currentHealth <= 0)
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == other.GetComponent<CapsuleCollider2D>() && isAttacking && other.gameObject.tag == "Boss")
        {
            Boss.instance.TakeDamage();
        }
        else if (other == other.GetComponent<CapsuleCollider2D>() && isAttacking && other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<Snail>().health > 1)
            {
                other.gameObject.GetComponent<Snail>().LowerHealth();
            }
            else
            {
                other.gameObject.GetComponent<Snail>().Die();
            }
        }
        else if (other == other.GetComponent<CircleCollider2D>() && isAttacking && other.gameObject.tag == "FlyingEnemy")
        {
            if (other.gameObject.GetComponent<Bee>().health > 1)
            {
                other.gameObject.GetComponent<Bee>().LowerHealth();
            }
            else
            {
                other.gameObject.GetComponent<Bee>().Kill();
            }
        }
        else
        {
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        attack_counter += Time.deltaTime;
        if (other == other.GetComponent<CapsuleCollider2D>() && isAttacking && attack_counter >= 1f  && other.gameObject.tag == "Boss")
        {
            Boss.instance.TakeDamage();
            attack_counter = 0;
        }
        else if (other == other.GetComponent<CapsuleCollider2D>() && isAttacking && attack_counter >= 1f  && other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<Snail>().health > 1)
            {
                other.gameObject.GetComponent<Snail>().LowerHealth();
            }
            else
            {
                other.gameObject.GetComponent<Snail>().Die();
            }
            attack_counter = 0;
        }
        else if (other == other.GetComponent<CircleCollider2D>() && isAttacking && attack_counter >= 1f  && other.gameObject.tag == "FlyingEnemy")
        {
            if (other.gameObject.GetComponent<Bee>().health > 1)
            {
                other.gameObject.GetComponent<Bee>().LowerHealth();
            }
            else
            {
                other.gameObject.GetComponent<Bee>().Kill();
            }
            attack_counter = 0;
        }
        else
        {
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Jumper"))
        {
            isGrounded = false;
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (StaminaHealth.instance.currentHealth <= 0)
        {
            isDead = true;
            anim.SetTrigger("Death");
            Invoke("ToHome", 1.12f);
            return;
        }
        else
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Snail>().Attack();
                StaminaHealth.instance.TakeDamage("Enemy");
            }
            if (other.gameObject.CompareTag("Trap"))
            {
                StaminaHealth.instance.TakeDamage("Trap");
            }
            if (other.gameObject.CompareTag("Boss"))
            {
                StaminaHealth.instance.TakeDamage("Boss");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        damage_counter += Time.deltaTime;
        if (damage_counter >= 2)
        {
            if (StaminaHealth.instance.currentHealth <= 0)
            {
                isDead = true;
                anim.SetTrigger("Death");
                Invoke("ToHome", 1.12f);
                isDead = false;
                return;
            }
            else
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    StaminaHealth.instance.TakeDamage("Enemy");
                }
                if (other.gameObject.CompareTag("Trap"))
                {
                    StaminaHealth.instance.TakeDamage("Trap");
                }
                if (other.gameObject.CompareTag("Boss"))
                {
                    StaminaHealth.instance.TakeDamage("Boss");
                }
                damage_counter = 0;
            }
        }
    }

    void ToHome()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(8);
        StaminaHealth.instance.Reset();
        isDead = false;
    }
}

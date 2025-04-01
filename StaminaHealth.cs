using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StaminaHealth : MonoBehaviour
{
    public Slider stamina;
    public int maxStamina = 100;
    public int currentStamina;
    public int staminaRegen = 2;

    public static StaminaHealth instance;

    public Slider health;
    public int maxHealth = 100;
    public int currentHealth;
    private int trapDamage = 10;
    private int enemyDamage = 15;
    private int bossDamage = 20;

    static bool isDone = false;

    public int cscore = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Animator anim;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (isDone)
        {
            currentStamina = PlayerPrefs.GetInt("stamina");
            stamina.maxValue = maxStamina;
            stamina.value = currentStamina;
            currentHealth = PlayerPrefs.GetInt("health");
            health.maxValue = maxHealth;
            health.value = currentHealth;
            cscore = PlayerPrefs.GetInt("score");
            scoreText.text = cscore.ToString();
        }
        else
        {
            currentStamina = maxStamina;
            PlayerPrefs.SetInt("stamina", currentStamina);
            stamina.maxValue = maxStamina;
            stamina.value = currentStamina;
            currentHealth = maxHealth;
            PlayerPrefs.SetInt("health", currentHealth);
            health.maxValue = maxHealth;
            health.value = currentHealth;
            cscore = 0;
            scoreText.text = cscore.ToString();
            isDone = true;
        }
    }

    void Update()
    {
        scoreText.text = cscore.ToString();
    }

    public void UseStamina()
    {
        if (currentStamina-5 > 0)
        {
            currentStamina -= 5;
            stamina.value = currentStamina;
        }
        else
        {
            Debug.Log("Not enough stamina!");
        }
        PlayerPrefs.SetInt("stamina", currentStamina);
    }

    public void GainStamina()
    {
        if (currentStamina+staminaRegen < maxStamina)
        {
            currentStamina += staminaRegen;
            stamina.value = currentStamina;
        }
        else
        {
            currentStamina = maxStamina;
            stamina.value = currentStamina;
        }
        PlayerPrefs.SetInt("stamina", currentStamina);
    }

    public void SpecificStamina(int stam)
    {
        if (currentStamina+stam < maxStamina)
        {
            currentStamina += stam;
            stamina.value = currentStamina;
        }
        else
        {
            currentStamina = maxStamina;
            stamina.value = currentStamina;
        }
        PlayerPrefs.SetInt("stamina", currentStamina);
    }

    public void TakeDamage(string entity)
    {
        if (currentHealth > 0)
        {
            if (entity == "Enemy")
            {
                currentHealth -= enemyDamage;
            }
            else if (entity == "Boss")
            {
                currentHealth -= bossDamage;
            }
            else
            {
                currentHealth -= trapDamage;
            }
            health.value = currentHealth;
            anim.SetTrigger("Hurt");
        }
        PlayerPrefs.SetInt("health", currentHealth);
    }

    public void GainHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 10;
            health.value = currentHealth;
        }
        else
        {
            currentHealth = maxHealth;
            health.value = currentHealth;
        }
        PlayerPrefs.SetInt("health", currentHealth);
    }

    public void GainPoints()
    {
        cscore += 10;
        scoreText.text = cscore.ToString();
        PlayerPrefs.SetInt("score", cscore);
    }

    public void FoodPoints(int food)
    {
        cscore += (20 * food);
        scoreText.text = cscore.ToString();
        PlayerPrefs.SetInt("score", cscore);
    }

    public void Reset()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        health.maxValue = maxHealth;
        health.value = currentHealth;
        cscore = 0;
        isDone = false;
    }
}


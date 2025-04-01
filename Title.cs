using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Title : MonoBehaviour
{
    public Slider volume;
    public static bool isPaused = false;
    public static bool isShopped = false;
    public GameObject pauseMenuUI;
    public GameObject shopMenuUI;
    public static Title instance;

    public Button hButton;
    public Button sButton;

    public int quantity = 0;
    public TMP_Text quantityText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        volume.value = PlayerPrefs.GetFloat("volume");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        try
        {
            SetVolume();
        }
        catch
        {
            return;
        }
    }

    public void Resume()
    {
        if (isPaused)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ToShop()
    {
        shopMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isShopped = true;
    }

    public void BuyC()
    {
        if (StaminaHealth.instance.cscore >= 50 && StaminaHealth.instance.currentHealth < StaminaHealth.instance.maxHealth)
        {
            StaminaHealth.instance.cscore -= 50;
            quantity++;
            quantityText.text = quantity.ToString();
            StaminaHealth.instance.GainHealth();
        }
        if (StaminaHealth.instance.currentHealth >= StaminaHealth.instance.maxHealth)
        {
            StaminaHealth.instance.currentHealth = StaminaHealth.instance.maxHealth;
        }
        StaminaHealth.instance.health.maxValue = StaminaHealth.instance.maxHealth;
        StaminaHealth.instance.health.value = StaminaHealth.instance.currentHealth;
    }

    public void BuyS()
    {
        if (StaminaHealth.instance.cscore >= 100)
        {
            StaminaHealth.instance.cscore -= 100;
            StaminaHealth.instance.maxStamina += 50;
            StaminaHealth.instance.staminaRegen = 5;
            StaminaHealth.instance.currentStamina = StaminaHealth.instance.maxStamina;
            sButton.interactable = false;
        }
        StaminaHealth.instance.stamina.maxValue = StaminaHealth.instance.maxStamina;
        StaminaHealth.instance.stamina.value = StaminaHealth.instance.currentStamina;
    }

    public void BuyH()
    {
        if (StaminaHealth.instance.cscore >= 500)
        {
            StaminaHealth.instance.cscore -= 500;
            StaminaHealth.instance.maxHealth += 50;
            StaminaHealth.instance.currentHealth = StaminaHealth.instance.maxHealth;
            hButton.interactable = false;
        }
        StaminaHealth.instance.health.maxValue = StaminaHealth.instance.maxHealth;
        StaminaHealth.instance.health.value = StaminaHealth.instance.currentHealth;
    }

    public void ExitShop()
    {
        shopMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isShopped = false;
    }

    public void ToTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(2);
    }

    public void Instructions()
    {
        SceneManager.LoadScene(1);
    }

    public void SetVolume()
    {
        AudioListener.volume = volume.value;
        PlayerPrefs.SetFloat("volume", volume.value);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
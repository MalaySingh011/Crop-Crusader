using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//Manages Dialogue
public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public GameObject dialogueManager;

    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public static bool isTalking = false;
    public static bool teleporter = false;
    public static bool tporter = false;

    int enemies;

    //Starts a new Dialogue Manager
    void Start()
    {
        sentences = new Queue<string>();
        enemies = FindObjectsByType<Snail>(FindObjectsSortMode.None).Length;
    }

    //Accepts a Dialogue class input and displays it
    public void StartDialogue (Dialogue dialogue)
    {
        sentences.Clear();
        StaminaHealth.instance.SpecificStamina(10);
        dialogueManager.SetActive(true);
        nameText.text = dialogue.name;
        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            if (dialogue.sentences[i].Contains("^"))
            {
                Snail.totalkills += Snail.currkills;
                dialogue.sentences[i] = dialogue.sentences[i].Replace("z", Snail.currkills.ToString());
                Snail.currkills = 0;
            }
            if (dialogue.sentences[i].Contains("~"))
            {
                StaminaHealth.instance.FoodPoints(Food.foodCount);
                Food.totalFoodCount += Food.foodCount;
                dialogue.sentences[i] = dialogue.sentences[i].Replace("x", Food.foodCount.ToString());
                Food.foodCount = 0;
            }
            sentences.Enqueue(dialogue.sentences[i]);
            if (dialogue.sentences[i].Contains("teleport"))
            {
                teleporter = true;
                tporter = false;
            }
            if (dialogue.sentences[i].Contains("take"))
            {
                teleporter = false;
                tporter = true;
            }
        }
        isTalking = true;
        DisplayNextSentence();
    }

    //Displays the next sentence
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    //Types a sentence letter by letter
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    //Stops dialogue
    void EndDialogue()
    {
        dialogueManager.SetActive(false);
        isTalking = false;
        if (teleporter)
        {
            Invoke("LoadNextScene", 2f);
            teleporter = false;
        }
        if (tporter)
        {
            Invoke("LoadOtherScene", 2f);
            tporter = false;
        }
        if (SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 6)
        {
            SceneManager.LoadScene(7);
        }
    }
    
    //Loads the next scene
    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Loads another scene
    void LoadOtherScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
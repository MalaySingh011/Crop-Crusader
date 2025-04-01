using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;

    public static int killCount;
    public static int cropCount;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}

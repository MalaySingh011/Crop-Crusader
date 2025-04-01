using UnityEngine;
using TMPro;

public class WinLose : MonoBehaviour
{
    public TMP_Text statsText;

    void Start()
    {
        statsText.text = statsText.text.Replace("x", Food.totalFoodCount.ToString());
        statsText.text = statsText.text.Replace("z", Snail.totalkills.ToString());
    }
}

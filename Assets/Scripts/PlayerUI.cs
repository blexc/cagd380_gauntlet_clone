using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private TextMeshProUGUI healthText = null;
    [SerializeField] private TextMeshProUGUI keysText = null;
    [SerializeField] private TextMeshProUGUI potionsText = null;

    public void Start()
    {
        HideText();
    }

    public void HideText()
    {
        scoreText.enabled = false;
        healthText.enabled = false;
        keysText.enabled = false;
        potionsText.enabled = false;
    }

    public void ShowText()
    {
        scoreText.enabled = true;
        healthText.enabled = true;
        keysText.enabled = true;
        potionsText.enabled = true;
    }

    public void UpdateText(int score, int health, int keys, int potions)
    {
        scoreText.text = "Score: " + score.ToString();
        healthText.text = "Health: " + health.ToString();
        keysText.text = "Keys: " + keys.ToString();
        potionsText.text = "Potions: " + potions.ToString();
    }
}

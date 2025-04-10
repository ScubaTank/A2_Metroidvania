using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _scoreText;

    public void UpdateUI(int health, int score){
        _healthText.text = "Health: " + health;
        _scoreText.text = "Score: " + score;
    }
}

using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;

    ScoreKeeper scoreKeeper;
    void Awake()
    {
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowFinalScore() {
        finalScoreText.text = $"Your Score: {scoreKeeper.CalculateScore()}%";
    }
}

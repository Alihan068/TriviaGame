using UnityEngine;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour {
    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 5f;


    public bool isAnsweringQuestion = false;

    public float fillFraction;

    public bool loadNextQuestion;

    float timerValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        UpdateTimer();
    }

    void UpdateTimer() {
        timerValue -= Time.deltaTime;
        if (isAnsweringQuestion) {
            if (timerValue > 0) {
                fillFraction = timerValue / timeToCompleteQuestion;
            }
            else {
                timerValue = timeToShowCorrectAnswer;
                isAnsweringQuestion = false;
            }
        }
        else {
            if (timerValue > 0) {
                fillFraction = timerValue / timeToShowCorrectAnswer;
            }
            else {
                isAnsweringQuestion = true;
                timerValue = timeToCompleteQuestion;
                loadNextQuestion = true;
            }
        }
    }

    public void CancelTimer() {
        timerValue = 0;
    }
}






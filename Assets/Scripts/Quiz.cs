using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour {

    [Header("Question")]
    [SerializeField] QuestionSO questionSO;
    [SerializeField] TextMeshProUGUI questionText;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    public bool hasAnsweredEarly;

    [Header("ButtonColors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        timer = FindFirstObjectByType<Timer>();
        GetNextQuestion();
    }

    // Update is called once per frame
    void Update() {
        timerImage.fillAmount = timer.fillFraction;

        if (timer.loadNextQuestion) {
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion) {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index) {
        hasAnsweredEarly = true;

        DisplayAnswer(index);

        SetButtonState(false);
        timer.CancelTimer();
    }

    void DisplayQuestion() {
        questionText.text = questionSO.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++) {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = questionSO.GetAnswer(i);
        }
    }

    void GetNextQuestion() {
        SetButtonState(true);
        SetDefaultButtonSprites();
        DisplayQuestion();
    }
    void SetButtonState(bool state) {
        for (int i = 0; i < answerButtons.Length; i++) {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites() {
        for (int i = 0; i < answerButtons.Length; i++) {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
    void DisplayAnswer(int index) {
        Image buttonSprite;

        if (index == questionSO.GetCorrectAnswerIndex()) {
            questionText.text = "Correct!";
            buttonSprite = answerButtons[index].GetComponent<Image>();
            buttonSprite.sprite = correctAnswerSprite;
        }
        else {
            correctAnswerIndex = questionSO.GetCorrectAnswerIndex();
            string correctAnswer = questionSO.GetAnswer(correctAnswerIndex);
            questionText.text = "Wrong! The correct answer is: \n" + correctAnswer;
            buttonSprite = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonSprite.sprite = correctAnswerSprite;
        }
    }
}

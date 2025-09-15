using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour {

    [Header("Question")]
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    [HideInInspector] public QuestionSO currentQuestion;
    [SerializeField] TextMeshProUGUI questionText;

    [Header("Answers")]
    [HideInInspector] public GameObject[] answerButtons;
    int correctAnswerIndex;
    [HideInInspector] public bool hasAnsweredEarly = true;
    
    
    
    [Header("ButtonColors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;

    [HideInInspector] public bool isComplete;

    HintButton hintButton;

    void Awake() {
        hintButton = FindFirstObjectByType<HintButton>();
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        scoreText.text = "Score 0%";

        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update() {
        timerImage.fillAmount = timer.fillFraction;

        if (timer.loadNextQuestion) {
            if (progressBar.value == progressBar.maxValue) {
                isComplete = true;
            }
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion) {
            DisplayAnswer(questions.Count + 1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index) {
        hasAnsweredEarly = true;

        DisplayAnswer(index);

        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score " + scoreKeeper.CalculateScore() + "%";
    }

    void DisplayQuestion() {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++) {
            GameObject selectedButton = answerButtons[i];
            selectedButton.SetActive(true);

            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    void GetNextQuestion() {
        if (questions.Count > 0) {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            hintButton.ResetHintFeatures();
            scoreKeeper.IncrementQuestionsSeen();
            progressBar.value++;          
        }
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

    void GetRandomQuestion() {
        int index = UnityEngine.Random.Range(0, questions.Count);

        currentQuestion = questions[index];

        if (questions.Contains(currentQuestion)) {
            questions.Remove(currentQuestion);
        }
    }
    void DisplayAnswer(int index) {
        Image buttonSprite;
        if (index == currentQuestion.GetCorrectAnswerIndex()) {
            questionText.text = "Correct!";
            buttonSprite = answerButtons[index].GetComponent<Image>();
            buttonSprite.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else {
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex);
            questionText.text = "Wrong! The correct answer is: \n" + correctAnswer;
            buttonSprite = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonSprite.sprite = correctAnswerSprite;
        }
        hintButton.DisableHintButton();
    }
 
}


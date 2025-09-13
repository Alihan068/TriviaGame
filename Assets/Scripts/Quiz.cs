using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour {

    [Header("Question")]
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;
    [SerializeField] TextMeshProUGUI questionText;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    [HideInInspector] public bool hasAnsweredEarly = true;

    [Header("Hint")]
    [SerializeField] GameObject hintButton;
    [SerializeField] int perQuestionHintAmount = 2;
    [SerializeField] int totalHintAmount = 5;
    [SerializeField] List<Button> answerButton;
    [HideInInspector] public List<int> alreadyChoiced;

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

    void Awake() {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        scoreText.text = "Score 0%";

        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    // Update is called once per frame
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
            ClearHintList();
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
    }
    public void OnHintButtonPressed() {
        if ((perQuestionHintAmount > 0) && (alreadyChoiced.Count < answerButtons.Length)){
            DestroyRandomAnswer();
            perQuestionHintAmount -= 1;
            totalHintAmount -= 1;
            if (perQuestionHintAmount == 0) {
                hintButton.SetActive(false);
            }
        }
        else {
            Debug.Log("No Remaining Hints");
        }

    }
    void DestroyRandomAnswer() {
        int choice = UnityEngine.Random.Range(0, answerButtons.Length);
        TextMeshProUGUI selectedButton = answerButtons[RandomNotCorrectAnswerIndex(choice)].GetComponentInChildren<TextMeshProUGUI>();
        Destroy(selectedButton);
    }

    int RandomNotCorrectAnswerIndex(int choice) { //Give a random buttonsIndex from one of the wrong answers.       
        Debug.Log("alreadyChoiced has " + alreadyChoiced.Count + " Members ");
        while (alreadyChoiced.Contains(choice)) {
            choice = UnityEngine.Random.Range(0, answerButtons.Length);
            Debug.Log("Choice is = " + choice);
        }
        alreadyChoiced.Add(choice);
        Debug.Log("Choice = " + choice + " added to the list");
        Debug.Log("alreadyChoiced now have " + alreadyChoiced.Count + " Members ");
        return choice;
        //Transform.childcount
    }
    void ClearHintList() {
        alreadyChoiced.Clear();
        alreadyChoiced.Add(currentQuestion.GetCorrectAnswerIndex());
    }
}


using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintButton : MonoBehaviour
{
    [SerializeField] Quiz quiz;

    [Header("HintButton")]
    [SerializeField] GameObject hintButton;
    [SerializeField] TextMeshProUGUI hintButtonText;
    [SerializeField] int perQuestionHintAmount = 2;
    [SerializeField] int totalHintAmount = 5;
    [HideInInspector] public List<int> alreadyChoiced;

    public void EnableHintButton()
    {
        hintButton.SetActive(true);
    }
    public void DisableHintButton()
    {
        hintButton.SetActive(false);
    }

    public void OnHintButtonPressed()
    {
        if ((perQuestionHintAmount > 0) && (alreadyChoiced.Count < quiz.answerButtons.Length))
        {
            DestroyRandomAnswer();
            perQuestionHintAmount -= 1;
            totalHintAmount -= 1;
            hintButtonText.text = "Hint = " + perQuestionHintAmount.ToString();
            if (perQuestionHintAmount == 0)
            {
                DisableHintButton();
            }
        }
        else
        {
            Debug.Log("No Remaining Hints");
            DisableHintButton();
        }

    }

    public void ResetHintFeatures()
    {
        alreadyChoiced.Clear();
        alreadyChoiced.Add(quiz.currentQuestion.GetCorrectAnswerIndex());
        Debug.Log("Correct Index = " + quiz.currentQuestion.GetCorrectAnswerIndex());
        perQuestionHintAmount = quiz.answerButtons.Length - 2;
        hintButtonText.text = "Hint = " + perQuestionHintAmount.ToString();
        if (totalHintAmount > 0)
        {
            EnableHintButton();
        }
    }

    public int RandomNotCorrectAnswerIndex(int choice)
    { //Give a random buttonsIndex from one of the wrong answers.       
        while (alreadyChoiced.Contains(choice))
        {
            choice = UnityEngine.Random.Range(0, quiz.answerButtons.Length);
        }
        alreadyChoiced.Add(choice);
        return choice;
        //Transform.childcount
    }

    void DestroyRandomAnswer()
    {
        int choice = UnityEngine.Random.Range(0, quiz.answerButtons.Length);
        GameObject selectedButton = quiz.answerButtons[RandomNotCorrectAnswerIndex(choice)];
        selectedButton.SetActive(false);
    }



}

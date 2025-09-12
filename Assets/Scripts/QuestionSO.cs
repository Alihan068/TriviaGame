using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2,6)]  
    [SerializeField] string question = "Enter new question";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex;

    public string GetQuestion() {
        return question; // Example Question?
    }
    public string GetAnswer(int index) {
        return answers[index]; // 0 = "Answer"", 1 = "Answer2", 2= "Correct Answer", 3 = "Answer4"
    }

    public int GetCorrectAnswerIndex() { 
        return correctAnswerIndex; // return 2(correct answer)
    }

}

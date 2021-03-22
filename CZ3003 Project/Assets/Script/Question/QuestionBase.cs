using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Question", menuName = "Question/Create new Question")]
public class QuestionBase : ScriptableObject
{
    [TextArea]
    [SerializeField] string question;

    [SerializeField] string questionDifficulty;

    [SerializeField] string answer;
    [SerializeField] string wrongAnswer1;
    [SerializeField] string wrongAnswer2;
    
    //[SerializeField] string sectionNumber;

    public string Question {
        get{ return question; }
    }

    public string QuestionDifficulty {
        get { return questionDifficulty; }
    }

    public string Answer {
        get { return answer; }
    }

    public string WrongAnswer1 {
        get { return wrongAnswer1; }
    }

    public string WrongAnswer2 {
        get { return wrongAnswer2; }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "QuestionClass", menuName = "Question/Create new Question Class")]
public class Question  : ScriptableObject
{
   [SerializeField] List<QuestionBase> questionBank;

   public List<QuestionBase> QB {
        get { return questionBank; }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "SurveyQuestion")]
public class SurveyQuestion : ScriptableObject
{
    public string QuestionTitle;
    public string PropertyName;
}

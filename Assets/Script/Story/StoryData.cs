using System;
using UnityEngine;

[Serializable]
public class StoryData
{
    public float StoryLength;
    public Sprite StoryContent;

    [Header("Survey")] 
    public bool IsSurvey;
    public SurveyType SurveyType;
}

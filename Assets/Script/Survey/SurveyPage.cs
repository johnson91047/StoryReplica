using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SurveyPage : MonoBehaviour
{
    public GameObject NormalQuestionPrefab;
    public Transform QuestionParent;
    public List<SurveyQuestion> Questions;
    public Button CompleteButton;
    public Action OnSurveyComplete;
    public bool IsSurveyTwo;

    private List<QuestionElement> _questionElements;
    private void Start()
    {
        _questionElements = new List<QuestionElement>();

        CompleteButton.onClick.AddListener(OnCompleteClicked);

        InitQuestionElements();
    }

    private void OnCompleteClicked()
    {
        if (CheckComplete())
        {
            SetSurveyData();
            if (IsSurveyTwo)
            {
                ToPersonalSurvey();
            }
            else
            {
                SurveyState.CurrentSurvey.StartTimer();
                BackToStudy();
            }

        }
        else
        {
            // TODO show message
        }

    }

    private void ToPersonalSurvey()
    {
        SceneManager.LoadScene(4);
    }

    private void BackToStudy()
    {
        SceneManager.LoadScene(SceneState.SavedSceneNum);
    }

    private void SetSurveyData()
    {
        List<SurveyData> data = new List<SurveyData>();


        foreach (QuestionElement questionElement in _questionElements)
        {
            data.Add(questionElement.GetData());
        }

        if (IsSurveyTwo)
        {
            SurveyState.CurrentSurvey.Survey2 = data;
        }
        else
        {
            SurveyState.CurrentSurvey.Survey1 = data;
        }
        
    }

    private bool CheckComplete()
    {
        foreach (QuestionElement questionElement in _questionElements)
        {
            if (!questionElement.IsComplete())
            {
                return false;
            }
        }

        return true;
    }

    private void InitQuestionElements()
    {
        foreach (SurveyQuestion surveyQuestion in Questions)
        {
            NormalQuestion question = Instantiate(NormalQuestionPrefab, QuestionParent)
                .GetComponent<NormalQuestion>();
            question.Title.text = surveyQuestion.QuestionTitle;
            _questionElements.Add(question);
        }
    }
}

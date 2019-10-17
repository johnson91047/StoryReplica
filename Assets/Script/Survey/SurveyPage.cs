using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyPage : MonoBehaviour
{
    public GameObject NormalQuestionPrefab;
    public Transform QuestionParent;
    public List<SurveyQuestion> Questions;
    public Button CompleteButton;
    public Action OnSurveyComplete;

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
            UploadData();
            OnSurveyComplete?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            // TODO show message
        }

    }

    private void UploadData()
    {
        //TODO upload data to firebase
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
            switch (surveyQuestion.QuestionType)
            {
                case SurveyQuestionType.Normal:
                    NormalQuestion question = Instantiate(NormalQuestionPrefab, QuestionParent)
                        .GetComponent<NormalQuestion>();
                    question.Title.text = surveyQuestion.QuestionTitle;
                    _questionElements.Add(question);
                    break;
                case SurveyQuestionType.Selective:
                    //TODO add selective
                    break;
            }
        }
    }
}

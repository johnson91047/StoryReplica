using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveySystem : MonoBehaviour
{
    public static SurveySystem Instance;

    public GameObject Survey1;
    public GameObject Survey2;
    public GameObject PersonalData;
    public GameObject StarterGuide;

    private Action _callBack;
    private Dictionary<int, bool> _surveyCheck;
    private SurveyPage _currentSurveyPage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
        _surveyCheck = new Dictionary<int, bool>();
    }

    public void RegisterStorySurvey(int id, bool value)
    {
        _surveyCheck.Add(id,value);
    }

    public bool CheckStorySurvey(int id)
    {
        return _surveyCheck[id];
    }


    public void ShowPage(SurveyType type, Action completeCallBack)
    {
        switch (type)
        {
            case SurveyType.Survey1:
                _currentSurveyPage = Instantiate(Survey1, transform).GetComponent<SurveyPage>();
                break;
            case SurveyType.Survey2:
                Instantiate(Survey2, transform);
                break;
            case SurveyType.Personal:
                Instantiate(PersonalData, transform);
                break;
            case SurveyType.Starter:
                Instantiate(StarterGuide, transform);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        _currentSurveyPage.OnSurveyComplete += completeCallBack;
    }
}

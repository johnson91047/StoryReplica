using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PersonalSurveyPage : MonoBehaviour
{
    public GameObject FirstPage;
    public GameObject SeconPage;
    public GameObject ThirdPage;
    public GameObject FourthPage;
    public GameObject NormalQuestionPrefab;

    public Transform ControlSurveyParent;

    public ToggleGroup SexToggleGroup;
    public ToggleGroup AgeToggleGroup;
    public ToggleGroup EducationToggleGroup;
    public ToggleGroup ClassToggleGroup;


    public Button NextButton;

    public List<GameObject> PersonalQuestions;
    public List<SurveyQuestion> Study1ControlQuestions;
    public List<SurveyQuestion> Study2ControlQuestions;
    public List<SurveyQuestion> Study3ControlQuestions;

    private int _count;
    private List<QuestionElement> _elements;

    void Start()
    {
        _elements = new List<QuestionElement>();
        FirstPage.SetActive(true);
        SeconPage.SetActive(false);
        ThirdPage.SetActive(false);
        FourthPage.SetActive(false);
        NextButton.onClick.AddListener(ButtonClick);
        _count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async void ButtonClick()
    {
        switch (_count)
        {
            case 0:
                FirstPage.SetActive(false);
                SeconPage.SetActive(true);
                _count++;
                break;
            case 1:
                if (CheckPersonalAnswer())
                {
                    SetPersonalData();
                    SeconPage.SetActive(false);
                    ThirdPage.SetActive(true);
                    InitQuestions();
                    _count++;
                }
                break;
            case 2:
                if (CheckControlSurveyAnswer())
                {
                    SetControlData();
                    await UploadData();
                    ThirdPage.SetActive(false);
                    FourthPage.SetActive(true);
                    NextButton.gameObject.SetActive(false);
                }
                break;
        }
    }

    private bool CheckPersonalAnswer()
    {
        foreach (GameObject question in PersonalQuestions)
        {
            ToggleGroup toggleGroup = question.GetComponent<ToggleGroup>();
            if (!toggleGroup.AnyTogglesOn())
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckControlSurveyAnswer()
    {
        foreach (QuestionElement questionElement in _elements)
        {
            if (!questionElement.IsComplete())
            {
                return false;
            }
        }

        return true;
    }

    private void SetControlData()
    {
        List<SurveyData> data = new List<SurveyData>();

        foreach (QuestionElement questionElement in _elements)
        {
            data.Add(questionElement.GetData());
        }

        SurveyState.CurrentSurvey.ControlSurvey = data;
    }

    private void InitQuestions()
    {
        string studyName = SurveyState.CurrentSurvey.StudyName.Split('_')[0];
        switch (studyName)
        {
            case "Study1":
                foreach (SurveyQuestion surveyQuestion in Study1ControlQuestions)
                {
                    NormalQuestion question = Instantiate(NormalQuestionPrefab, ControlSurveyParent)
                        .GetComponent<NormalQuestion>();
                    question.Title.text = surveyQuestion.QuestionTitle;
                    _elements.Add(question);
                }
                break;
            case "Study2":
                foreach (SurveyQuestion surveyQuestion in Study2ControlQuestions)
                {
                    NormalQuestion question = Instantiate(NormalQuestionPrefab, ControlSurveyParent)
                        .GetComponent<NormalQuestion>();
                    question.Title.text = surveyQuestion.QuestionTitle;
                    _elements.Add(question);
                }
                break;
            case "Study3":
                foreach (SurveyQuestion surveyQuestion in Study3ControlQuestions)
                {
                    NormalQuestion question = Instantiate(NormalQuestionPrefab, ControlSurveyParent)
                        .GetComponent<NormalQuestion>();
                    question.Title.text = surveyQuestion.QuestionTitle;
                    _elements.Add(question);
                }
                break;
        }
    }
    private void SetPersonalData()
    {
        PersonalSurvey personal = new PersonalSurvey
        {
            Sex = SexToggleGroup.GetActivatedToggleTranform().GetComponent<ToggleComponent>().PropertyName,
            Age = AgeToggleGroup.GetActivatedToggleTranform().GetComponent<ToggleComponent>().PropertyName,
            Education = EducationToggleGroup.GetActivatedToggleTranform().GetComponent<ToggleComponent>().PropertyName,
            Class = ClassToggleGroup.GetActivatedToggleTranform().GetComponent<ToggleComponent>().PropertyName
        };

        SurveyState.CurrentSurvey.SetTime();
        SurveyState.CurrentSurvey.PersonalData = personal;
    }

    private async Task UploadData()
    {
        await FireBaseManager.FireBaseClient.Child("Survey")
            .PostAsync(JsonConvert.SerializeObject(SurveyState.CurrentSurvey));
    }
}

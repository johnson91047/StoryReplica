using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class PersonalSurveyPage : MonoBehaviour
{
    public GameObject FirstPage;
    public GameObject SeconPage;
    public GameObject ThirdPage;

    public ToggleGroup SexToggleGroup;
    public ToggleGroup AgeToggleGroup;
    public ToggleGroup EducationToggleGroup;
    public ToggleGroup ClassToggleGroup;


    public Button NextButton;

    public List<GameObject> Questions;

    private int _count;

    void Start()
    {
        FirstPage.SetActive(true);
        SeconPage.SetActive(false);
        ThirdPage.SetActive(false);
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
                if (CheckAnswer())
                {
                    await UploadData();
                    SeconPage.SetActive(false);
                    ThirdPage.SetActive(true);
                    NextButton.gameObject.SetActive(false);
                }
                
                break;
        }
    }

    private bool CheckAnswer()
    {
        foreach (GameObject question in Questions)
        {
            ToggleGroup toggleGroup = question.GetComponent<ToggleGroup>();
            if (!toggleGroup.AnyTogglesOn())
            {
                return false;
            }
        }

        return true;
    }

    private async Task UploadData()
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

        await FireBaseManager.FireBaseClient.Child("Survey")
            .PostAsync(JsonConvert.SerializeObject(SurveyState.CurrentSurvey));
    }
}

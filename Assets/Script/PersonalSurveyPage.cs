using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersonalSurveyPage : MonoBehaviour
{
    public GameObject FirstPage;
    public GameObject SeconPage;
    public GameObject ThirdPage;
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

    private void ButtonClick()
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
                    UploadData();
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

    private void UploadData()
    {
        //TODO Upload data
    }
}

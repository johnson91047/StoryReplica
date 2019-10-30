using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneState.CurrentPageNum = 0;
        SceneState.SavedSceneNum = 0;
        SceneState.TimeValue = 0;
        SurveyState.IsFinishedSurveyOne = false;
        SurveyState.ShouldSavePageCount = true;
        SurveyState.ShouldSaveTimeCount = true;
        SurveyState.CurrentSurvey = new Survey();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartDemoButton : MonoBehaviour
{
    public string StudyName;
    public int SceneNumber;

    private Button _button;

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(StartDemoScene);
    }

    private void StartDemoScene()
    {
        SurveyState.CurrentSurvey.StudyName = StudyName;
        PlayerPrefs.SetInt("TargetStudyScene",SceneNumber);
        SceneManager.LoadScene(1);
    }
}

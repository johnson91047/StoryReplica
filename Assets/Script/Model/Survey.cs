using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

[Serializable]
public class Survey
{
    public string StudyName;

    public int ViewedPageCount = 0; 

    public float ViewTime = 0f;

    public List<SurveyData> Survey1 { get; set; } = new List<SurveyData>();
    public List<SurveyData> Survey2 { get; set; } = new List<SurveyData>();

    public PersonalSurvey PersonalData;

    private Stopwatch _stopwatch;

    public void StartTimer()
    {
        if (_stopwatch == null)
        {
            Debug.Log("Start new");
            _stopwatch = Stopwatch.StartNew();
        }
        else
        {
            Debug.Log("Start");
            _stopwatch.Start();
        }
    }

    public void StopTimer()
    {
        Debug.Log("Pause");
        _stopwatch.Stop();
    }

    public void SetTime()
    {
        ViewTime = _stopwatch.ElapsedMilliseconds / 1000f;
    }
}
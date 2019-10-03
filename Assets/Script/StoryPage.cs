using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryPage : MonoBehaviour
{
    public GameObject TimeBarPrefab;
    public Transform TimeBarParent;
    public Image StoryContent;
    public int CurrentPageIndex;
    public Action OnChangeToNextStory;
    public Action OnChangeToPreviousStory;
    public Action<bool> OnChangeToFirstStory;

    [HideInInspector]
    public StoryObject StoryObject;

    private bool _isPersistent;
    private bool _pause;
    private Slider _currentTimeBar;
    private float _currentPageTimeCount;
    private int _totalPageIndex;
    private List<Slider> _timeBars;

    // Start is called before the first frame update
    void Start()
    {
        _timeBars = new List<Slider>();
        _currentPageTimeCount = 0;
        _pause = true;
        _isPersistent = false;
        _totalPageIndex = StoryObject.Stories.Count;
        InitTimeBar();
        InitContent();
    }

    // Update is called once per frame
    void Update()
    {
        if (_pause || _isPersistent)
        {
            return;
        }

        if (_currentPageTimeCount >= _currentTimeBar.maxValue)
        {
            NextPage();
            return;
        }
        else if (_currentPageTimeCount < _currentTimeBar.maxValue)
        {
            _currentPageTimeCount += Time.deltaTime;
        }

        _currentTimeBar.value = _currentPageTimeCount;
    }

    public void SetPause(bool pause)
    {
        _pause = pause;
    }

    public void StayCurrentState()
    {
        _timeBars[CurrentPageIndex].value = 0;
        _currentPageTimeCount = 0;
        SetPause(true);
    }

    public void NextPage()
    {
        if (_isPersistent)
        {
            OnChangeToFirstStory?.Invoke(true);
            return;
        }

        CurrentPageIndex++;

        if (CurrentPageIndex == _totalPageIndex)
        {
            CurrentPageIndex = _totalPageIndex - 1;
            _timeBars[CurrentPageIndex].value = 0;
            _currentPageTimeCount = 0;
            SetPause(true);

            OnChangeToNextStory?.Invoke();
            return;
        }

        _timeBars[CurrentPageIndex-1].value = _timeBars[CurrentPageIndex-1].maxValue;


        SetCurrentContent();
        SetCurrentTimeBar();
    }

    public void PreviousPage()
    {
        if (_isPersistent)
        {
            OnChangeToPreviousStory?.Invoke();
            return;
        }

        CurrentPageIndex--;

        if (CurrentPageIndex < 0)
        {
            CurrentPageIndex = 0;
            _timeBars[CurrentPageIndex].value = 0;
            _currentPageTimeCount = 0;
            SetPause(true);

            OnChangeToPreviousStory?.Invoke();
            return;
        }

        for (int i = 0; i < _timeBars.Count; i++)
        {
            if (i < CurrentPageIndex)
            {
                _timeBars[i].value = _timeBars[i].maxValue;
            }
            else
            {
                _timeBars[i].value = 0;
            }
        }

        SetCurrentContent();
        SetCurrentTimeBar();
    }

    public void ResetPage()
    {
        foreach (Slider timeBar in _timeBars)
        {
            timeBar.value = 0;
        }

        CurrentPageIndex = 0;
        SetCurrentContent();
        SetCurrentTimeBar();

        SetPause(true);
    }

    private void SetCurrentContent()
    {
        StoryContent.sprite = StoryObject.Stories[CurrentPageIndex].StoryContent;
    }

    private void SetCurrentTimeBar()
    {
        _currentPageTimeCount = 0;
        _currentTimeBar = _timeBars[CurrentPageIndex];
        _currentTimeBar.value = 0;
    }

    private void InitContent()
    {
        StoryContent.sprite = StoryObject.Stories[0].StoryContent;
    }

    private void InitTimeBar()
    {
        if (Math.Abs(StoryObject.Stories[0].StoryLength - (-1)) < float.Epsilon)
        {
            _isPersistent = true;
        }

        foreach (StoryData story in StoryObject.Stories)
        {
            GameObject timeBarGameObject = Instantiate(TimeBarPrefab, TimeBarParent);
            Slider timeBar = timeBarGameObject.GetComponent<Slider>();
            timeBar.minValue = 0;
            timeBar.maxValue = story.StoryLength;
            timeBar.value = 0;
            _timeBars.Add(timeBar);
        }

        _currentTimeBar = _timeBars[0];
    }

}

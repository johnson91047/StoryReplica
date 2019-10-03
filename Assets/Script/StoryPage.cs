using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StoryPage : MonoBehaviour
{
    public GameObject TimeBarPrefab;
    public Transform TimeBarParent;
    public Image StoryContent;
    public int CurrentPageIndex;
    public int UnControlableImageIndex;
    public Action OnChangeToNextStory;
    public Action OnChangeToPreviousStory;
    public Action OnPersistentNext;

    [HideInInspector]
    public StoryObject StoryObject;

    private bool _isPersistent;
    private bool _pause;
    private Slider _currentTimeBar;
    private float _currentPageTimeCount;
    private float _uncontrolableTimeCount;
    private int _totalPageIndex;
    private List<Slider> _timeBars;

    // Start is called before the first frame update
    void Start()
    {
        _timeBars = new List<Slider>();
        _currentPageTimeCount = 0;
        _uncontrolableTimeCount = 0;
        UnControlableImageIndex = 0;
        _pause = true;
        _isPersistent = false;
        _totalPageIndex = StoryObject.Controlable ? StoryObject.Stories.Count : 1;
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
            Debug.Log("Next page");
            return;
        }

        if (_currentPageTimeCount < _currentTimeBar.maxValue)
        {
            _uncontrolableTimeCount += Time.deltaTime;
            _currentPageTimeCount += Time.deltaTime;

            if (!StoryObject.Controlable)
            {
                if (_uncontrolableTimeCount >= StoryObject.TotalLength / StoryObject.Stories.Count)
                {
                    NextImage();
                    Debug.Log("Next Image");
                    _uncontrolableTimeCount = 0;
                }
            }
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
        UnControlableImageIndex = 0;
        _uncontrolableTimeCount = 0;
        SetCurrentContent();
        SetPause(true);
    }

    public void NextPage()
    {
        if (_isPersistent)
        {
            OnPersistentNext?.Invoke();
            return;
        }

        CurrentPageIndex++;

        if (CurrentPageIndex >= _totalPageIndex)
        {
            CurrentPageIndex = _totalPageIndex - 1;
            _timeBars[CurrentPageIndex].value = 0;
            _currentPageTimeCount = 0;
            _uncontrolableTimeCount = 0;
            UnControlableImageIndex = 0;
            SetPause(true);

            OnChangeToNextStory?.Invoke();
            return;
        }

        _timeBars[CurrentPageIndex-1].value = _timeBars[CurrentPageIndex-1].maxValue;


        SetCurrentContent();
        SetCurrentTimeBar();
    }

    public void NextImage()
    {
        UnControlableImageIndex++;

        if (UnControlableImageIndex >= StoryObject.Stories.Count)
        {
            return;
        }

        StoryContent.sprite = StoryObject.Stories[UnControlableImageIndex].StoryContent;
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
            _uncontrolableTimeCount = 0;
            UnControlableImageIndex = 0;
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
        UnControlableImageIndex = 0;
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
        _uncontrolableTimeCount = 0;
    }

    private void InitContent()
    {
        StoryContent.sprite = StoryObject.Stories[0].StoryContent;
    }

    private void InitTimeBar()
    {
        if (StoryObject.Persistent)
        {
            _isPersistent = true;
        }

        if (StoryObject.Controlable)
        {
            foreach (StoryData story in StoryObject.Stories)
            {
                GameObject timeBarGameObject = Instantiate(TimeBarPrefab, TimeBarParent);
                Slider timeBar = timeBarGameObject.GetComponent<Slider>();
                timeBar.minValue = 0;
                timeBar.maxValue = story.StoryLength;
                timeBar.value = 0;
                _timeBars.Add(timeBar);
            }
        }
        else
        {
            GameObject timeBarGameObject = Instantiate(TimeBarPrefab, TimeBarParent);
            Slider timeBar = timeBarGameObject.GetComponent<Slider>();
            timeBar.minValue = 0;
            timeBar.maxValue = StoryObject.TotalLength;
            timeBar.value = 0;
            _timeBars.Add(timeBar);
        }


        _currentTimeBar = _timeBars[0];
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryMaster : MonoBehaviour
{
    public GameObject StoryPagePrefab;
    public int CurrentStoryIndex = 0;

    public List<StoryObject> Stories;

    private int _totalStoriesCount;
    private Camera _mainCamera;
    private CameraController _cameraController;
    private List<StoryPage> _pages;
    public StoryPage CurrentPage;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _cameraController = _mainCamera.transform.GetComponent<CameraController>();
        _pages = new List<StoryPage>();

        InitStories();
        _totalStoriesCount = transform.childCount;

        StartCoroutine(SetCameraToCurrentPagePosInit());
    }

    private IEnumerator SetCameraToCurrentPagePosInit()
    {
        yield return new WaitForEndOfFrame();
        Vector3 pos = transform.GetChild(CurrentStoryIndex).transform.position;
        _cameraController.SetDestination(pos);
        CurrentPage = _pages[CurrentStoryIndex];
        SetCurrentPagePause(false);
    }

    private void SetCameraToCurrentPagePos()
    {
        CurrentStoryIndex = Mathf.Clamp(CurrentStoryIndex, 0, transform.childCount);
        Vector3 pos = transform.GetChild(CurrentStoryIndex).transform.position;
        _cameraController.SetDestination(pos);
        CurrentPage = _pages[CurrentStoryIndex];
        SetCurrentPagePause(false);
    }

    private void InitStories()
    {
        foreach (StoryObject storyObject in Stories)
        {
            GameObject storyPageGameObject = Instantiate(StoryPagePrefab, transform);
            StoryPage page = storyPageGameObject.GetComponent<StoryPage>();
            page.StoryObject = storyObject;
            page.OnChangeToNextStory += ChangeToNextStory;
            page.OnChangeToPreviousStory += ChangeToPreviousStory;
            page.OnPersistentNext += BackToMenu;
            _pages.Add(page);
        }
    }

    public void ChangeToNextStory()
    {
        CurrentStoryIndex++;

        if (CurrentStoryIndex == _totalStoriesCount)
        {
            SceneManager.LoadScene(Stories[0].HasSurveyTwo ? 3 : 4);

            return;
        }

        CurrentPage.StayCurrentState();
        SetCameraToCurrentPagePos();
        Debug.Log("Next Story");
    }

    public void ChangeToPreviousStory()
    {
        CurrentStoryIndex--;

        if (CurrentStoryIndex < 0)
        {
            CurrentStoryIndex = 0;
            StayAtCurrentStory();
            return;
        }

        CurrentPage.StayCurrentState();

        SetCameraToCurrentPagePos();
    }

    public void StayAtCurrentStory()
    {
        SetCameraToCurrentPagePos();
    }

    public void SetCurrentPagePause(bool pause)
    {
        CurrentPage.SetPause(pause);
    }

    public void ChangeToNextPage()
    {
        CurrentPage.NextPage();
    }

    public void ChangeToPreviousPage()
    {
        CurrentPage.PreviousPage();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }


    public void ResetAllStories()
    {
        foreach (StoryPage storyPage in _pages)
        {
            storyPage.ResetPage();
        }
    }
}

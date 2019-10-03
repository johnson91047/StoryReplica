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
    private StoryPage _currentPage;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _cameraController = _mainCamera.transform.GetComponent<CameraController>();
        _pages = new List<StoryPage>();

        InitStories();
        _totalStoriesCount = transform.childCount;

        StartCoroutine(SetCameraToCurrentPagePos());
    }

    private IEnumerator SetCameraToCurrentPagePos()
    {
        yield return new WaitForEndOfFrame();
        Vector3 pos = transform.GetChild(CurrentStoryIndex).transform.position;
        _cameraController.SetDestination(pos);
        _currentPage = _pages[CurrentStoryIndex];
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
            CurrentStoryIndex = _totalStoriesCount - 1;
            StayAtCurrentStory();
            return;
        }

        _currentPage.StayCurrentState();
        StartCoroutine(SetCameraToCurrentPagePos());
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

        _currentPage.StayCurrentState();

        StartCoroutine(SetCameraToCurrentPagePos());
    }

    public void StayAtCurrentStory()
    {
        StartCoroutine(SetCameraToCurrentPagePos());
    }

    public void SetCurrentPagePause(bool pause)
    {
        _currentPage.SetPause(pause);
    }

    public void ChangeToNextPage()
    {
        _currentPage.NextPage();
    }

    public void ChangeToPreviousPage()
    {
        _currentPage.PreviousPage();
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

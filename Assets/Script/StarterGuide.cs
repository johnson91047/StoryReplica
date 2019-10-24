using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarterGuide : MonoBehaviour
{
    public int CountDown;
    public Button NextButton;
    public TextMeshProUGUI ButtonText;
    public GameObject FirstPage;
    public GameObject SecondPage;
    public GameObject StartPage;

    private int _currentCountDown;

    private int _imageCount;
    // Start is called before the first frame update
    void Start()
    {
        FirstPage.SetActive(true);
        SecondPage.SetActive(false);
        StartPage.SetActive(false);
        NextButton.onClick.AddListener(ButtonClicked);
        _currentCountDown = CountDown;
        _imageCount = 0;
        StartCoroutine(CountDownTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ButtonClicked()
    {
        _imageCount++;

        switch (_imageCount)
        {
            case 1:
                FirstPage.SetActive(false);
                SecondPage.SetActive(true);
                StartCoroutine(CountDownTimer());
                break;
            case 2:
                SecondPage.SetActive(false);
                StartPage.SetActive(true);
                NextButton.gameObject.SetActive(false);
                StartCoroutine(ChangeToStudy());
                break;
        }
    }

    private IEnumerator ChangeToStudy()
    {
        yield return new WaitForSeconds(3);
        int sceneNum = PlayerPrefs.GetInt("TargetStudyScene");
        SceneManager.LoadScene(sceneNum);
    }

    private IEnumerator CountDownTimer()
    {
        NextButton.interactable = false;

        while (_currentCountDown > 0)
        {
            _currentCountDown -= 1;
            ButtonText.text = _currentCountDown.ToString();
            yield return new WaitForSeconds(1);
        }

        ButtonText.text = "Next";
        _currentCountDown = CountDown;
        NextButton.interactable = true;
    }
}

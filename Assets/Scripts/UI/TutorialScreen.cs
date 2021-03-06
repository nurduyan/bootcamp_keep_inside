using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : MonoBehaviour{
    [SerializeField] private GameObject[] _UIElementsToHide;
    [SerializeField] private Button _leftFreezeButton;
    [SerializeField] private Button _rightFreezeButton;
    [SerializeField] private Button _rewardedAdButton;
    [SerializeField] private GameObject _introductionScreen;
    [SerializeField] private GameObject _firstTutorialScreen;
    [SerializeField] private GameObject _secondTutorialScreen;
    [SerializeField] private GameObject _thirdTutorialScreen;
    [SerializeField] private GameObject _lastScreen;


    private void Start(){
        if(PlayerPrefs.GetInt("first_start") == 1){
            ShowTutorialScreen();
            PlayerPrefs.SetInt("first_start", 0);
        }
    }


    private void ShowTutorialScreen(){
        foreach (GameObject UIElement in _UIElementsToHide){
            UIElement.SetActive(false);
        }
        _leftFreezeButton.interactable = false;
        _rightFreezeButton.interactable = false;
        _rewardedAdButton.interactable = false;
        Time.timeScale = 0;
        foreach (PaddleController paddle in FindObjectsOfType<PaddleController>()){
            paddle.DisableControls();
        }
        _introductionScreen.SetActive(true);
    }
    public void ShowFirstScreen(){
        _introductionScreen.SetActive(false);
        _firstTutorialScreen.SetActive(true);
    }
    public void ShowSecondScreen(){
        _firstTutorialScreen.SetActive(false);
        _secondTutorialScreen.SetActive(true);
    }
    public void ShowThirdScreen(){
        _secondTutorialScreen.SetActive(false);
        _leftFreezeButton.gameObject.SetActive(true);
        _rightFreezeButton.gameObject.SetActive(true);
        _thirdTutorialScreen.SetActive(true);
    }
    public void ShowLastScreen(){
        _thirdTutorialScreen.SetActive(false);
        _leftFreezeButton.gameObject.SetActive(false);
        _rightFreezeButton.gameObject.SetActive(false);
        _rewardedAdButton.gameObject.SetActive(true);
        _lastScreen.SetActive(true);
    }
    public void SkipTutorialScreen(){
        _introductionScreen .SetActive(false);
        _firstTutorialScreen.SetActive(false);
        _secondTutorialScreen.SetActive(false);
        _thirdTutorialScreen.SetActive(false);
        CloseTutorialScreen();
    }
    public void CloseTutorialScreen(){
        foreach (GameObject UIElement in _UIElementsToHide){
            UIElement.SetActive(true);
        }
        _lastScreen.SetActive(false);
        _leftFreezeButton.interactable = true;
        _rightFreezeButton.interactable = true;
        _rewardedAdButton.interactable = true;
        Time.timeScale = 1;
        foreach (PaddleController paddle in FindObjectsOfType<PaddleController>()){
            paddle.EnableControls(1);
        }
    }
}

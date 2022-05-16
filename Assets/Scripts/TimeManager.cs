using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI _timerUIText;
    [SerializeField] private float _remainingTime;
    private bool _timerRunning = false;
    private int _paddleCount;
    private int _startRequest = 0;

    private void Start(){
        _paddleCount = FindObjectsOfType<PaddleController>().Length;
    }
    public void StartTimer(){
        _startRequest++;
        if(_startRequest >= _paddleCount){
            _timerRunning = true;
        }
    }
    public void StopTimer(){
        _timerRunning = false;
    }
    public void AddTime(float amount){
        _remainingTime += amount;
    }

    public void SubtractTime(float amount){
        _remainingTime -= amount;
    }

    private void Update(){
        if(_timerRunning){
            _remainingTime -= Time.deltaTime;
            _timerUIText.text = "= " + (int)_remainingTime;
            if(_remainingTime <= 0){
                _timerUIText.text = "Time is up!";
                SceneManager.LoadScene("RetryGameScene");
            }
        }
    }
}

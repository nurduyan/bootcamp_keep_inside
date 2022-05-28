using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour,IDataPersistence{
    [SerializeField] private TextMeshProUGUI _timerUIText;
    [SerializeField] private float _remainingTime;
    private bool _timerRunning = false;
    private int _paddleCount;
    private int _startRequest = 0;
    private GameObject _stopper = null;

    private void Start(){
        _paddleCount = FindObjectsOfType<PaddleController>().Length;
        _timerUIText.text = " " + (int)_remainingTime;
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
    public void StopTimerBy(GameObject stopper){
        _timerRunning = false;
        _stopper = stopper;
    }
    public GameObject GetStopper(){
        return _stopper;
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
            _timerUIText.text = " " + (int)_remainingTime;
            if(_remainingTime <= 1){
                _timerUIText.text = "Time is up!";
                DataPersistenceManager.Instance?.SaveGame();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
    public void LoadData(GameData data){
    }
    public void SaveData(GameData data){
        data._lastLevelIndex = Mathf.Min(SceneManager.GetActiveScene().buildIndex + 1, 13);
    }
}

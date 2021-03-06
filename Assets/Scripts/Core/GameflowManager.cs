using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameflowManager : MonoBehaviour, IDataPersistence{
    [SerializeField] private TextMeshProUGUI _timerUIText;
    [SerializeField] private TextMeshProUGUI _remainingFreezeCountText;
    [SerializeField] private float _remainingTime;
    [SerializeField] private int _startingFreezeBallsCount;
    [SerializeField] private int _rewardedAdFreezeRewardAmount;
    [SerializeField] private GameObject _levelCompleteScreen;

    private int _remainingFreezeBallsCount;
    private int _paddleCount;
    private int _startRequest = 0;
    private bool _timerRunning = false;
    private bool _gameStarted = false;
    private GameObject _starter = null;
    private GameObject _stopper = null;

    private void Awake(){
        Time.timeScale = 1;
    }
    private void Start(){
        if(DataPersistenceManager.Instance != null){
            DataPersistenceManager.Instance.LoadGame();
        }

        if(AdsManager.Instance != null){
            AdsManager.Instance.LoadInterstitialAd();
            AdsManager.Instance.LoadRewardedAd();
        }

        _remainingFreezeBallsCount = _startingFreezeBallsCount;
        _paddleCount = FindObjectsOfType<PaddleController>().Length;
        _timerUIText.text = ((int)_remainingTime).ToString();
        _remainingFreezeCountText.text = ": " + _remainingFreezeBallsCount;
    }
    public int GetFreezeBallsCount(){
        return _remainingFreezeBallsCount;
    }
    public int GetRemainingTime(){
        return (int)_remainingTime;
    }
    public void UseFreezeBalls(){
        _remainingFreezeBallsCount = Mathf.Max(0, _remainingFreezeBallsCount - 1);
        _remainingFreezeCountText.text = ": " + _remainingFreezeBallsCount;
    }
    public void AddFreezeBallsCount(){
        _remainingFreezeBallsCount += _rewardedAdFreezeRewardAmount;
        _remainingFreezeCountText.text = ": " + _remainingFreezeBallsCount;
    }
    public void StartTimerBy(GameObject starter){
        if(_starter != starter || _gameStarted){
            _starter = starter;
            _startRequest++;
            if(_startRequest >= _paddleCount){
                _timerRunning = true;
                _gameStarted = true;
            }
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

    public void PlayRewardedAd(){
        if(AdsManager.Instance != null){
            AdsManager.Instance.ShowRewardedAd();
        }
    }
    private void Update(){
        if(_timerRunning){
            _remainingTime -= Time.deltaTime;
            _timerUIText.text = ((int)_remainingTime).ToString();
            if(_remainingTime <= 0){
                _timerUIText.text = "";
                if(!_levelCompleteScreen.activeSelf){
                    AudioListener.volume = 0;
                }

                Time.timeScale = 0;
                foreach (PaddleController paddle in FindObjectsOfType<PaddleController>()){
                    paddle.DisableControls();
                }
                _levelCompleteScreen.SetActive(true);
            }
        }
    }
    public void LoadData(GameData data){
    }
    public void SaveData(GameData data){
        data._lastLevelIndex = Mathf.Min(SceneManager.GetActiveScene().buildIndex + 1, SceneManager.sceneCountInBuildSettings - 3);
    }
}

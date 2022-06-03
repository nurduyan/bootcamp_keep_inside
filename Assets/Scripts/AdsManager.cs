using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener{

    [SerializeField] private string _androidGameId;
    [SerializeField] private bool _testMode;
    [SerializeField] private int _maxLevelOrDeathCountBeforeShowingAds;

    private int _levelOrDeathCountSinceLastAd = 0;
    private string _interstitialAdId = "Interstitial_Android";
    private string _rewardedAdId = "Rewarded_Android";
    private bool _interstitialAdLoaded = false;
    private bool _rewardedAdLoaded = false;
    public static AdsManager Instance{ get; private set; }
    private void Awake(){
        if(Instance != null){
            Debug.Log("Found more than one Ads Manager in the scene. Destroying the newest one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAds();
    }
    
    private void InitializeAds(){
        Advertisement.Initialize(_androidGameId, _testMode, this);
    }

    public void LoadInterstitialAd(){
        if(!_interstitialAdLoaded){
            Advertisement.Load(_interstitialAdId, this);
        }
    }
    public void IncLevelPassOrDeath(){
        _levelOrDeathCountSinceLastAd++;
        if(_levelOrDeathCountSinceLastAd >= _maxLevelOrDeathCountBeforeShowingAds){
            ShowInterstitialAd();
            _levelOrDeathCountSinceLastAd = 0;
        }
    }
    public void ShowInterstitialAd(){
        if(_interstitialAdLoaded){
            Advertisement.Show(_interstitialAdId, this);
            _interstitialAdLoaded = false;
        }
    }
    public void LoadRewardedAd(){
        if(!_rewardedAdLoaded){
            Advertisement.Load(_rewardedAdId, this);
        }
    }
    public void ShowRewardedAd(){
        if(_rewardedAdLoaded){
            Advertisement.Show(_rewardedAdId, this);
        }
    }

    public void OnUnityAdsAdLoaded(string placementId){
        if(placementId.Equals(_interstitialAdId)){
            _interstitialAdLoaded = true;
        }
        else if(placementId.Equals(_rewardedAdId)){
            _rewardedAdLoaded = true;
        }
    }
    public void OnInitializationComplete(){
        Debug.Log("Ads initialized!");
    }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message){
        throw new NotImplementedException();
    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message){
        throw new System.NotImplementedException();
    }
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message){
        throw new System.NotImplementedException();
    }
    public void OnUnityAdsShowStart(string placementId){
        AudioListener.volume = 0;
        Time.timeScale = 0;
    }
    public void OnUnityAdsShowClick(string placementId){
        throw new System.NotImplementedException();
    }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState){
        if(placementId == _rewardedAdId && (int)showCompletionState == (int)UnityAdsCompletionState.COMPLETED){
            RewardUser();
            _rewardedAdLoaded = false;
        }
        else if(placementId.Equals(_interstitialAdId)){
            _interstitialAdLoaded = false;
        }
        Debug.Log("Successfully completed showing ads");
        AudioListener.volume = DataPersistenceManager.Instance.LoadVolumePref();
        Time.timeScale = 1;
    }

    private void RewardUser(){
        FindObjectOfType<GameflowManager>().AddFreezeBallsCount();
    }
    
}

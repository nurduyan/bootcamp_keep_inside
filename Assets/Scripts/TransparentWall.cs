using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransparentWall : MonoBehaviour{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            if(AdsManager.Instance != null){
                AdsManager.Instance.IncLevelPassOrDeath();
            }

            GameflowManager gameflowManager = FindObjectOfType<GameflowManager>();
            FirebaseAnalytics.LogEvent("died", "remaining_time", gameflowManager != null ? gameflowManager.GetRemainingTime() : -1);
            SceneManager.LoadScene("RetryGameScene");
        }
    }
}

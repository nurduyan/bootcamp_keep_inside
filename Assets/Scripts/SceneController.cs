using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadMainMenu(){
        SceneManager.LoadScene(0);
    }
    public void ReplayLevel(){
        if(AdsManager.Instance != null){
            AdsManager.Instance.IncLevelPassOrDeath();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadNextLevel(){
        if(AdsManager.Instance != null){
            AdsManager.Instance.IncLevelPassOrDeath();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

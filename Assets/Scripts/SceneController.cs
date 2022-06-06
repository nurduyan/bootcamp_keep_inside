using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadMainMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void ReplayLevel(){
        if(DataPersistenceManager.Instance != null){
            AudioListener.volume = DataPersistenceManager.Instance.LoadVolumePref();
        }
        if(AdsManager.Instance != null){
            AdsManager.Instance.IncLevelPassOrDeath();
        }

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadNextLevel(){
        if(DataPersistenceManager.Instance != null){
            DataPersistenceManager.Instance.SaveGame();
            AudioListener.volume = DataPersistenceManager.Instance.LoadVolumePref();
        }
        if(AdsManager.Instance != null){
            AdsManager.Instance.IncLevelPassOrDeath();
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour, IDataPersistence{
    private int _levelToLoad;
    private void Start(){
        DataPersistenceManager.Instance.LoadGame();
    }
    public void SetQuality(int qual)
    {
        QualitySettings.SetQualityLevel(qual);
    }

    public void ReturnButton()
{
        SceneManager.LoadScene(0);

}
    public void StartGameButton()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
    public void StartNewGame(){
        DataPersistenceManager.Instance.StartNewGame();
        StartGameButton();
    }

    public void OptionsGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void DeveloperGameButton()
    {
        SceneManager.LoadScene(17);
    }
        
    public void QuitGameButton()
    {
        Debug.Log("ciktin");
        Application.Quit();
    }
    
    public void LoadData(GameData data){
        _levelToLoad = data._lastLevelIndex;
    }
    public void SaveData(GameData data){
        //Nothing to save here
    }

}

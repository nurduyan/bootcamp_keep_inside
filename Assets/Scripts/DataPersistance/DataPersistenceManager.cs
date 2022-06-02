using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = true;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    [SerializeField] private bool useEncryption;

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;

    public static DataPersistenceManager Instance{ get; private set; }

    private void Awake(){
        if(Instance != null){
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }
    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
    }
    
    public void NewGame(){
        _gameData = new GameData();
    }

    public void LoadGame(){
        // load any saved data from a file using the data handler
        _gameData = _dataHandler.Load();

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if(_gameData == null && initializeDataIfNull){
            NewGame();
        }

        // if no data can be loaded, don't continue
        if(_gameData == null){
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects){
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame(){
        // if we don't have any data to save, log a warning here
        if(_gameData == null){
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects){
            dataPersistenceObj.SaveData(_gameData);
        }

        // save that data to a file using the data handler
        _dataHandler.Save(_gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects(){
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

}

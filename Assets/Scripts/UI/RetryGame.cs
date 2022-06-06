using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryGame : MonoBehaviour, IDataPersistence{
    private int _lastLevelIndex = 0;
    private void Start(){
        if(DataPersistenceManager.Instance != null){
            DataPersistenceManager.Instance.LoadGame();
        }
    }
    
    public void LoadData(GameData data){
        _lastLevelIndex = data._lastLevelIndex;
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(_lastLevelIndex);
    }
    public void SaveData(GameData data){
        // Nothing to save here...
    }
}

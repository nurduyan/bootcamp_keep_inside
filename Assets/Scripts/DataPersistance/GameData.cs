using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public int _lastLevelIndex;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData(){
        _lastLevelIndex = 2;
    }
}

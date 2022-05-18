using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CoinManager : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private int _levelCompleteReward;
    private int _coinCount = 0;

    private void Start(){
        UpdateCoinText();
    }
    public void AddCoin(int amount){
        _coinCount += amount;
        UpdateCoinText();
    }
    public void AddCoinOnLevelComplete(){
        _coinCount += _levelCompleteReward;
        UpdateCoinText();
    }
    public int GetCoinCount(){
        return _coinCount;
    }
    private void UpdateCoinText(){
        _coinText.text = _coinCount.ToString();
    }
    
}

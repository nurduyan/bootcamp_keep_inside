using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour{
    private TextMeshProUGUI _fpsText;
    private float _deltaTime;
    private void Awake(){
        _fpsText = GetComponent<TextMeshProUGUI>();
    }
    void Update(){
        _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
        float fps = 1.0f / _deltaTime;
        _fpsText.text = Mathf.Ceil(fps).ToString();
    }
}

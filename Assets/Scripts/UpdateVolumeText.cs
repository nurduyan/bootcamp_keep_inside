using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpdateVolumeText : MonoBehaviour{
    [SerializeField] private Slider _volumeSlider;
    private TextMeshProUGUI _volumeText;
    private void Awake(){
        _volumeText = GetComponent<TextMeshProUGUI>();
    }
    private void Start(){
        float volume = DataPersistenceManager.Instance.LoadVolumePref();
        _volumeSlider.value = volume;
        UpdateVolume(volume);
    }
    public void UpdateVolume(float volume){
        _volumeText.text = ((int)(volume * 100)).ToString();
        DataPersistenceManager.Instance.SaveVolumePref(volume);
        AudioListener.volume = volume;
    }
}

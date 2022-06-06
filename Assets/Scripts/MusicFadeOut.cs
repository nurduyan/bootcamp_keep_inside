using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFadeOut : MonoBehaviour{
    [SerializeField] private float _fadeOutStartTime;
    [SerializeField] private float _fadeOutDuration;
    [SerializeField] private float _targetVolume;
    private void Start(){
        StartCoroutine(StartFade(GetComponent<AudioSource>()));
    }
    private IEnumerator StartFade(AudioSource audioSource){
        yield return new WaitForSeconds(_fadeOutStartTime);
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < _fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, _targetVolume, currentTime / _fadeOutDuration);
            yield return null;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFadeOut : MonoBehaviour{
    [SerializeField] private float _fadeOutStartTime;
    [SerializeField] private float _fadeOutDuration;
    [SerializeField] private float _targetVolume;

    private AudioSource _audioSource;
    private void Awake(){
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start(){
        StartCoroutine(StartFade());
    }
    private IEnumerator StartFade(){
        float currentTime = 0;
        yield return new WaitForSeconds(_fadeOutStartTime);
        float start = _audioSource.volume;
        while (currentTime < _fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(start, _targetVolume, currentTime / _fadeOutDuration);
            yield return null;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BallSpeedPickup : MonoBehaviour, IPickup {

    [SerializeField] private float _speedChange;
    [SerializeField] private float _pickupDuration;
    [SerializeField] private float _timeToDestroyWithoutPickup;

    private Ball _pickedupBall = null;//Bu pickupı triggerlayan top
    private SpawnArea _spawnArea;//Bulunduğumuz bölge
    private Coroutine _destroyCoroutine;
    
    private void Start(){
        _destroyCoroutine = StartCoroutine(DestroyAfterDuration());
    }
    //Interface methodu. Bu pickup instantiate edildikten sonra instantiate eden SpawnArea nesnesi tarafından çağrılır
    public void SetSpawnArea(SpawnArea spawnArea){
        _spawnArea = spawnArea;
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ball")){
            StopCoroutine(_destroyCoroutine);
            _pickedupBall = other.GetComponent<Ball>();
            StartCoroutine(ChangeSpeedForDuration());
        }
    }
    
    //Hızı belirtilen değer kadar değiştirir sonrasında nesneyi saklar ve belirtilen süre geçtikten sonra da hızı resetleyip kendisini yok
    //eder.
    IEnumerator ChangeSpeedForDuration(){
        _pickedupBall.ChangeSpeed(_speedChange);
        Hide();
        yield return new WaitForSeconds(_pickupDuration);
        _pickedupBall.ResetSpeed();
        Destroy(gameObject);
    }
    IEnumerator DestroyAfterDuration(){
        yield return new WaitForSeconds(_timeToDestroyWithoutPickup);
        Destroy(gameObject);
    }
    private void Hide(){
        GetComponent<MeshRenderer>().enabled = false;
        foreach (var renderer in GetComponentsInChildren<Renderer>()){
            renderer.enabled = false;
        }
        GetComponent<Collider>().enabled = false;
    }
}

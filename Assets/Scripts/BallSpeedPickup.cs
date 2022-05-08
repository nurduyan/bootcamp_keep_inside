using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BallSpeedPickup : MonoBehaviour, IPickup {

    [SerializeField] private float _speedChange;
    [SerializeField] private float _pickupDuration;

    private Ball _pickedupBall = null;//Bu pickupı triggerlayan top
    private SpawnArea _spawnArea;//Bulunduğumuz bölge
    
    //Interface methodu. Bu pickup instantiate edildikten sonra instantiate eden SpawnArea nesnesi tarafından çağrılır
    public void SetSpawnArea(SpawnArea spawnArea){
        _spawnArea = spawnArea;
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ball")){
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
    private void Hide(){
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}

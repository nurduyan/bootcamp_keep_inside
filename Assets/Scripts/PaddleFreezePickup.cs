using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleFreezePickup : MonoBehaviour, IPickup{
    [SerializeField] private float _pickupDuration;

    private SpawnArea _spawnArea;//Bulunduğumuz bölge
    private Move _paddleMove;//Bulunduğumuz bölgedeki paddle üzerindeki Move componentı
    
    //Interface methodu. Bu pickup instantiate edildikten sonra instantiate eden SpawnArea nesnesi tarafından çağrılır
    public void SetSpawnArea(SpawnArea spawnArea){
        _spawnArea = spawnArea;
    }
    private void Start(){
        _paddleMove = _spawnArea.GetAreaPaddle().GetComponent<Move>();
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ball")){
            StartCoroutine(FreezeForDuration());
        }
    }
    
    //Belirtilen süre boyunca paddleın hızını 0 yapar ve sonrasında eski haline getirir
    IEnumerator FreezeForDuration(){
        _paddleMove.ChangeSpeed(0);
        Hide();
        yield return new WaitForSeconds(_pickupDuration);
        _paddleMove.ResetSpeed();
        Destroy(gameObject);
    }
    private void Hide(){
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}

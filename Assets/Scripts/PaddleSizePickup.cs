using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PaddleSizePickup : MonoBehaviour, IPickup
{
    [SerializeField] private float _sizeChange;
    [SerializeField] private float _pickupDuration;

    private SpawnArea _spawnArea;//Bulunduğumuz bölge
    private PaddleController _paddle;//Bulunduğumuz bölgedeki paddle
    //Interface methodu. Bu pickup instantiate edildikten sonra instantiate eden SpawnArea nesnesi tarafından çağrılır
    public void SetSpawnArea(SpawnArea spawnArea){
        _spawnArea = spawnArea;
    }
    private void Start(){
        _paddle = _spawnArea.GetAreaPaddle();
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ball")){
            StartCoroutine(ChangeSizeForDuration());
        }
    }
    
    //Paddleın x eksenindeki boyutunu belirtilen değer oranında belirli süreliğine arttırır ve sonrasında eski haline getirir
    IEnumerator ChangeSizeForDuration(){
        Hide();
        _paddle.SetScale(_sizeChange);
        yield return new WaitForSeconds(_pickupDuration);
        _paddle.ResetScale();
        Destroy(gameObject);
    }

    
    private void Hide(){
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}

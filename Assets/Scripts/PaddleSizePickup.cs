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
    [SerializeField] private float _timeToDestroyWithoutPickup;

    private SpawnArea _spawnArea;//Bulunduğumuz bölge
    private PaddleController _paddle;//Bulunduğumuz bölgedeki paddle
    private Coroutine _destroyCoroutine;
    
    private void Start(){
        _destroyCoroutine = StartCoroutine(DestroyAfterDuration());
        _paddle = _spawnArea.GetAreaPaddle();
    }
    //Interface methodu. Bu pickup instantiate edildikten sonra instantiate eden SpawnArea nesnesi tarafından çağrılır
    public void SetSpawnArea(SpawnArea spawnArea){
        _spawnArea = spawnArea;
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ball")){
            StopCoroutine(_destroyCoroutine);
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

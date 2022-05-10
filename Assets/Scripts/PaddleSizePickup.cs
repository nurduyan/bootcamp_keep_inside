using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PaddleSizePickup : MonoBehaviour, IPickup
{
    [SerializeField] private float _sizeChange;
    [SerializeField] private float _pickupDuration;

    private SpawnArea _spawnArea;//Bulunduğumuz bölge
    private PaddleController _paddle;//Bulunduğumuz bölgedeki paddle
    private Vector3 _paddleStartingScale;//Resetleme işleminde kullanmak için paddleın ilk scaleini tutar
    
    //Interface methodu. Bu pickup instantiate edildikten sonra instantiate eden SpawnArea nesnesi tarafından çağrılır
    public void SetSpawnArea(SpawnArea spawnArea){
        _spawnArea = spawnArea;
    }
    private void Start(){
        _paddle = _spawnArea.GetAreaPaddle();
        _paddleStartingScale = _paddle.transform.localScale;
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ball")){StartCoroutine(ChangeSizeForDuration());
        }
    }
    
    //Paddleın x eksenindeki boyutunu belirtilen değer oranında belirli süreliğine arttırır ve sonrasında eski haline getirir
    IEnumerator ChangeSizeForDuration(){
        Vector3 currentPaddleScale = _paddle.transform.localScale;
        _paddle.transform.localScale = new Vector3(_sizeChange * currentPaddleScale.x, currentPaddleScale.y, currentPaddleScale.z);
        Hide();
        yield return new WaitForSeconds(_pickupDuration);
        _paddle.transform.localScale = _paddleStartingScale;
        Destroy(gameObject);
    }
    private void Hide(){
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}

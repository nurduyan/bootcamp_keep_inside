using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour, IPickup
{
    [SerializeField] private float _timeToDestroyWithoutPickup;
    [SerializeField] private int _coinToAdd;

    private SpawnArea _spawnArea;//Bulunduğumuz bölge
    private PaddleController _paddle;//Bulunduğumuz bölgedeki paddle
   
    private void Start(){
        StartCoroutine(DestroyAfterDuration());
        _paddle = _spawnArea.GetAreaPaddle();
    }
    //Interface methodu. Bu pickup instantiate edildikten sonra instantiate eden SpawnArea nesnesi tarafından çağrılır
    public void SetSpawnArea(SpawnArea spawnArea){
        _spawnArea = spawnArea;
    }
    
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ball")){
            FindObjectOfType<CoinManager>().AddCoin(_coinToAdd);
            Destroy(gameObject);
        }
    }
    IEnumerator DestroyAfterDuration(){
        yield return new WaitForSeconds(_timeToDestroyWithoutPickup);
        Destroy(gameObject);
    }
}

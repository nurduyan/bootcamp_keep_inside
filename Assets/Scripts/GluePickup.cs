using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluePickup : MonoBehaviour, IPickup
{
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
            _paddle.Glued(_pickupDuration);
            Destroy(gameObject);
        }
    }
}

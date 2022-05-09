using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DuplicatePickup : MonoBehaviour, IPickup{

    [SerializeField] private Ball _defaultBallPrefab;
    private Ball _pickedupBall = null;//Bu pickupı triggerlayan top
    private SpawnArea _spawnArea;//Bulunduğumuz bölge
    
    //Interface methodu. Bu pickup instantiate edildikten sonra instantiate eden SpawnArea nesnesi tarafından çağrılır
    public void SetSpawnArea(SpawnArea spawnArea){
        _spawnArea = spawnArea;
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ball")){
            _pickedupBall = other.GetComponent<Ball>();
            Vector3 newBallSpawnPos = new Vector3(transform.position.x, _pickedupBall.transform.position.y, transform.position.z);
            Ball newBall = Instantiate(_defaultBallPrefab, newBallSpawnPos, _pickedupBall.transform.rotation);
            newBall.Launch(new Vector3(Random.Range(-1f, 1f), 0,Random.Range(-1f, 1f)));
            Destroy(gameObject);
        }
    }
    
}

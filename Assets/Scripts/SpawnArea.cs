using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnArea : MonoBehaviour{
    [SerializeField] public PaddleController _paddle;
    [SerializeField] private Transform[] _pickupPrefabsToSpawn;
    [SerializeField] private float _timeBetweenSpawns;
    [SerializeField] private float _spawnHeightFromGround;

    private BoxCollider _boxCollider;
    
    //Bu bölgede bulunan paddleın get methodu
    public PaddleController GetAreaPaddle(){
        return _paddle;
    }
    private void Awake(){
        _boxCollider = GetComponent<BoxCollider>();
    }
    private void Start(){
        StartCoroutine(SpawnRandomPickup());
    }
    
    //Belirtilen süre aralığında sürekli olarak listeden random bir prefabi box collider içindeki random bir noktada spawnlar.
    IEnumerator SpawnRandomPickup(){
        while (true){
            int randomPickupIndex = Random.Range(0, _pickupPrefabsToSpawn.Length);
            Vector3 randomSpawnPos = GetRandomPointInsideCollider();
            Transform spawnedTransform = Instantiate(_pickupPrefabsToSpawn[randomPickupIndex], randomSpawnPos, Quaternion.identity);
            spawnedTransform.GetComponent<IPickup>().SetSpawnArea(this);
            yield return new WaitForSeconds(_timeBetweenSpawns);
        }
    }
    
    //Box colliderın içinde rasgele bir spawn noktası seçer
    private Vector3 GetRandomPointInsideCollider(){
        Vector3 extents = _boxCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            _spawnHeightFromGround,
            Random.Range(-extents.z, extents.z)
        );
        return _boxCollider.transform.TransformPoint(point);
    }
}

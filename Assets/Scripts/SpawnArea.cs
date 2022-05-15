using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnArea : MonoBehaviour{
    [SerializeField] private PaddleController _paddle;
    [SerializeField] private Transform[] _pickupPrefabsToSpawn;
    [SerializeField] private SpawnArea _otherSpawnArea;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    [SerializeField] private float _spawnHeightFromGround;
    [SerializeField] private float _maxRandomSpawnTryTime = 0.1f;

    private BoxCollider _boxCollider;
    private Coroutine _currentSpawnCoroutine;
    
    //Bu bölgede bulunan paddleın get methodu
    
    private void Awake(){
        _boxCollider = GetComponent<BoxCollider>();
    }
    private void Start(){
        _currentSpawnCoroutine = StartCoroutine(SpawnRandomPickup());
    }
    public PaddleController GetAreaPaddle(){
        return _paddle;
    }
    //Belirtilen süre aralığında sürekli olarak listeden random bir prefabi box collider içindeki random bir noktada spawnlar.
    IEnumerator SpawnRandomPickup(){
        while (true){
            float randomSpawnTime = Random.Range(_minSpawnTime, _maxSpawnTime);
            yield return new WaitForSeconds(randomSpawnTime);
            int randomPickupIndex = Random.Range(0, _pickupPrefabsToSpawn.Length);
            Vector3 randomSpawnPos = GetRandomPointInsideCollider();
            Transform spawnedTransform = Instantiate(_pickupPrefabsToSpawn[randomPickupIndex], randomSpawnPos, Quaternion.identity);
            Bounds colliderBounds = spawnedTransform.GetComponent<Collider>().bounds;
            float maxColliderSize = Math.Max(Mathf.Max(colliderBounds.extents.x, colliderBounds.extents.y), colliderBounds.extents.z);
            float lookUpTimer = 0f;
            while (Physics.OverlapSphere(randomSpawnPos, maxColliderSize, 1 << 3).Length > 0 && lookUpTimer < _maxRandomSpawnTryTime){
                randomSpawnPos = GetRandomPointInsideCollider();
                lookUpTimer += Time.deltaTime;
            }
            spawnedTransform.position = randomSpawnPos;
            spawnedTransform.GetComponent<IPickup>().SetSpawnArea(this);
            PortalPickup portal = spawnedTransform.GetComponent<PortalPickup>();
            if(portal != null){
                portal.SetOtherPortal(_otherSpawnArea.SpawnSecondPortal(portal));
            }
        }
    }
    public PortalPickup SpawnSecondPortal(PortalPickup portal){
        StopCoroutine(_currentSpawnCoroutine);
        Vector3 randomSpawnPos = GetRandomPointInsideCollider();
        PortalPickup spawnedPortal = Instantiate(portal, randomSpawnPos, portal.transform.rotation);
        spawnedPortal.SetSpawnArea(this);
        spawnedPortal.SetOtherPortal(portal);
        _currentSpawnCoroutine = StartCoroutine(SpawnRandomPickup());
        return spawnedPortal;
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

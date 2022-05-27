using System;
using System.Collections;
using System.Collections.Generic;
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

    private List<Ball> _ballsOnThisArea;
    private BoxCollider _boxCollider;
    private Coroutine _currentSpawnCoroutine;
    private Coroutine _currentFreezeCoroutine = null;

    private bool _spawnStarted = false;
    
    //Bu bölgede bulunan paddleın get methodu
    
    private void Awake(){
        _ballsOnThisArea = new List<Ball>();
        _boxCollider = GetComponent<BoxCollider>();
    }
    public void StartSpawning(){
        if(!_spawnStarted){
            _currentSpawnCoroutine = StartCoroutine(SpawnRandomPickup());
            _spawnStarted = true;
        }
    }
    public void AddBall(Ball ball){
        _ballsOnThisArea.Add(ball);
    }
    public void RemoveBall(Ball ball){
        _ballsOnThisArea.Remove(ball);
    }

    public void FreezeBalls(float duration){
        if(_ballsOnThisArea.Count > 0){
            if(_currentFreezeCoroutine != null){
                StopCoroutine(_currentFreezeCoroutine);
            }
            _currentFreezeCoroutine = StartCoroutine(FreezeBallsForDuration(duration));
        }
    }
    IEnumerator FreezeBallsForDuration(float duration){
        foreach (Ball ball in _ballsOnThisArea){
            if(!Mathf.Approximately(ball.GetSpeed(),0f)){
                ball.ChangeSpeed(0);
            }
        }
        TimeManager timeManager = FindObjectOfType<TimeManager>();
        timeManager.StopTimerBy(gameObject);
        yield return new WaitForSeconds(duration);
        foreach (Ball ball in _ballsOnThisArea){
            if(!ball.IsAttached()){
                ball.ResetSpeed();
            }
        }

        if(timeManager.GetStopper().GetComponent<SpawnArea>() == this){
            timeManager.StartTimer();
        }
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
            Transform spawnedTransform = Instantiate(_pickupPrefabsToSpawn[randomPickupIndex], randomSpawnPos, _pickupPrefabsToSpawn[randomPickupIndex].rotation);
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

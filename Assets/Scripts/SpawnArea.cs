using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnArea : MonoBehaviour{
    [SerializeField] public PaddleController _paddle;
    [SerializeField] private Transform[] _pickupPrefabsToSpawn;
    [SerializeField] private float _timeBetweenSpawns;
    [SerializeField] private float _spawnHeightFromGround;

    private BoxCollider _boxCollider;
    private void Awake(){
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Start(){
        StartCoroutine(SpawnRandomPickup());
    }

    IEnumerator SpawnRandomPickup(){
        int randomPickupIndex = Random.Range(0, _pickupPrefabsToSpawn.Length);
        Vector3 randomSpawnPos = GetRandomPointInsideCollider();
        Instantiate(_pickupPrefabsToSpawn[randomPickupIndex], randomSpawnPos, Quaternion.identity);
        yield return new WaitForSeconds(_timeBetweenSpawns);
        StartCoroutine(SpawnRandomPickup());
    }
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

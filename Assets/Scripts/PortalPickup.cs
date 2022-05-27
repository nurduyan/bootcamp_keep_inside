using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPickup : MonoBehaviour, IPickup
{
    [SerializeField] private float _timeToDestroyWithoutPickup;
    
    private SpawnArea _spawnArea; //Bulunduğumuz bölge
    private PortalPickup _otherPortal;

    private void Start(){
        StartCoroutine(DestroyAfterDuration());
    }
    public SpawnArea GetSpawnArea(){
        return _spawnArea;
    }
    public void SetOtherPortal(PortalPickup portal){
        _otherPortal = portal;
    }
    public void SetSpawnArea(SpawnArea spawnArea){
        _spawnArea = spawnArea;
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ball")){
            other.transform.position = new Vector3(_otherPortal.transform.position.x, other.transform.position.y,
                _otherPortal.transform.position.z);
            Ball triggeredBall = other.GetComponent<Ball>();
            if(transform.position.x < _otherPortal.transform.position.x){
                triggeredBall.UpdateSpeedAfterPortal(-90);
            }
            else{
                triggeredBall.UpdateSpeedAfterPortal(90);
            }
            _spawnArea.RemoveBall(triggeredBall);
            _otherPortal.GetSpawnArea().AddBall(triggeredBall);
            Destroy(_otherPortal.gameObject);
            Destroy(gameObject);
        }
    }
    IEnumerator DestroyAfterDuration(){
        yield return new WaitForSeconds(_timeToDestroyWithoutPickup);
        Destroy(gameObject);
    }
}

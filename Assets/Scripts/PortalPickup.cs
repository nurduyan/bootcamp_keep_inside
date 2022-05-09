using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPickup : MonoBehaviour, IPickup
{
    private SpawnArea _spawnArea; //Bulunduğumuz bölge
    private PortalPickup _otherPortal;

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
            Destroy(_otherPortal.gameObject);
            Destroy(gameObject);
        }
    }
}

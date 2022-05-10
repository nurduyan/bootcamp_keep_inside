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
            if(transform.position.x < _otherPortal.transform.position.x){
                other.GetComponent<Ball>().UpdateSpeedAfterPortal(-90);
            }
            else{
                other.GetComponent<Ball>().UpdateSpeedAfterPortal(90);
            }
            Destroy(_otherPortal.gameObject);
            Destroy(gameObject);
        }
    }
}

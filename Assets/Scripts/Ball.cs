using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour{
    
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _launchInterval;

    private Rigidbody _rigidbody;

    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void AttachBall(){
        _rigidbody.isKinematic = true;
    }
    public void LaunchBall(Vector3 forwardVectorToLaunch){
        transform.parent = null;
        _rigidbody.isKinematic = false;
        Vector3 launchDirection = Quaternion.AngleAxis(Random.Range(-_launchInterval, _launchInterval), Vector3.up) * forwardVectorToLaunch;
        _rigidbody.velocity = launchDirection * _moveSpeed;
    }
    /*private void OnCollisionExit(Collision other){
        _rigidbody.velocity = _moveSpeed * (_rigidbody.velocity.normalized);
    }*/
    
}

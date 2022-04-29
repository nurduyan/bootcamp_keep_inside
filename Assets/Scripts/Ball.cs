using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour{

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _launchInterval;

    private Rigidbody _rigidbody;
    private Vector3 _currentDirection = Vector3.zero;

    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void AttachBall(){
        _rigidbody.isKinematic = true;
    }
    public void Launch(Vector3 launchDirection){
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = launchDirection * _moveSpeed;
    }
}

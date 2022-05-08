using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour{

    [SerializeField] private float _startingSpeed;
    [SerializeField] private float _launchInterval;

    private float _moveSpeed;
    private Rigidbody _rigidbody;
    private Vector3 _currentDirection = Vector3.zero;

    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
        _moveSpeed = _startingSpeed;
    }
    public void AttachBall(){
        _rigidbody.isKinematic = true;
    }
    public void Launch(Vector3 launchDirection){
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = launchDirection * _moveSpeed;
    }

    public void ChangeSpeed(float changeAmount){
        _moveSpeed += changeAmount;
        UpdateVelocity();
    }

    public void ResetSpeed(){
        _moveSpeed = _startingSpeed;
        UpdateVelocity();
    }

    private void UpdateVelocity(){
        _rigidbody.velocity = _moveSpeed * _rigidbody.velocity.normalized;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Move : MonoBehaviour{
    [Header("Config")]
    [SerializeField] private float _startingSpeed;
    
    private Rigidbody _rigidbody;
    private float _movementInput = 0f;
    private float _moveSpeed;

    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
        _moveSpeed = _startingSpeed;
    }

    
    private void Update(){
        _movementInput = Input.GetAxisRaw("Horizontal");
        
    }

    private void FixedUpdate(){
        _rigidbody.velocity = transform.right * (_movementInput * _moveSpeed);
    }
    public void ChangeSpeed(float newSpeed){
        _moveSpeed = newSpeed;
    }
    public void ResetSpeed(){
        _moveSpeed = _startingSpeed;
    }
}

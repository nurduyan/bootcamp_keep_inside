using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Move : MonoBehaviour{
    [Header("Config")]
    [SerializeField] private float _moveSpeed;
    
    private Rigidbody _rigidbody;
    private float _movementInput = 0f;

    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
    }

    
    private void Update(){
        _movementInput = Input.GetAxisRaw("Horizontal");
        
    }

    private void FixedUpdate(){
        _rigidbody.velocity = transform.right * (_movementInput * _moveSpeed);
    }
}

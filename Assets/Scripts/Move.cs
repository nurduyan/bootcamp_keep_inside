using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Move : MonoBehaviour{
    [Header("Config")]
    [SerializeField] private float _moveSpeed;

    [SerializeField] private Ball _startingBall;
    
    private Rigidbody _rigidbody;
    private float _movementInput = 0f;

    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start(){
        _startingBall.AttachBall();
    }
    private void Update(){
        _movementInput = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _startingBall.LaunchBall(transform.forward);
        }
    }

    private void FixedUpdate(){
        _rigidbody.velocity = transform.right * (_movementInput * _moveSpeed);
    }
}

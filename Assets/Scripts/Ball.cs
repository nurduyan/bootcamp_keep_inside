using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour{

    [SerializeField] private float _startingSpeed;
    [SerializeField] private float _launchInterval;
    [Tooltip("How long should ball update its speed to avoid slowing down after collisions.(in seconds)")]
    [SerializeField] private float _speedUpdateRate;

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
        StartCoroutine(RetainCurrentSpeed());
    }

    public void ChangeSpeed(float changeAmount){
        _moveSpeed += changeAmount;
        UpdateVelocity();
    }
    
    public void ResetSpeed(){
        _moveSpeed = _startingSpeed;
        UpdateVelocity();
    }
    public void UpdateSpeedAfterPortal(float angleChange){
        _rigidbody.velocity = Quaternion.AngleAxis(angleChange, Vector3.up) * _rigidbody.velocity;
    }
    IEnumerator RetainCurrentSpeed(){
        while (true){
            UpdateVelocity();
            yield return new WaitForSeconds(_speedUpdateRate);
        }
    }
    private void UpdateVelocity(){
        _rigidbody.velocity = _moveSpeed * _rigidbody.velocity.normalized;
    }
}

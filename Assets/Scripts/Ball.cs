using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour{

    [SerializeField] private float _startingSpeed;
    [Tooltip("How long should ball update its speed to avoid slowing down after collisions.(in seconds)")]
    [SerializeField] private float _speedUpdateRate;

    private float _moveSpeed;
    private Rigidbody _rigidbody;
    private Vector3 _startingScale;
    private Vector3 _recordedVelocity = Vector3.zero;

    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
        _moveSpeed = 0f;
        _startingScale = transform.parent == null ? transform.localScale : Vector3.Scale(transform.localScale, transform.parent.localScale);
    }
    private void Start(){
        if(transform.parent != null){
            IgnoreCollisionWithPaddle(true);
        }
    }
    public float GetSpeed(){
        return _moveSpeed;
    }
    public bool IsAttached(){
        return transform.parent != null;
    }
    public void AttachBall(){
        IgnoreCollisionWithPaddle(true);
        _rigidbody.isKinematic = true;
        
    }
    public void Launch(Vector3 launchDirection){
        if(transform.parent != null){
            IgnoreCollisionWithPaddle(false);
            transform.parent = null;
        }

        _moveSpeed = _startingSpeed;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = launchDirection * _moveSpeed;
        StartCoroutine(RetainCurrentSpeed());
    }

    public void ChangeSpeed(float changeAmount){
        _moveSpeed *= changeAmount;
        if(Mathf.Approximately(_moveSpeed, 0f)){
            RecordVelocity();
        }
        UpdateVelocity();
    }
    
    public void ResetSpeed(){
        _moveSpeed = _startingSpeed;
        if(!_recordedVelocity.Equals(Vector3.zero)){
            RestoreVelocity();
        }
        UpdateVelocity();
    }
    public void ChangeScale(float amount){
        transform.localScale *= amount;
    }
    public void ResetScale(){
        if(transform.parent == null){
            transform.localScale = _startingScale;
        }
        else{
            Transform parent = transform.parent;
            transform.localScale = new Vector3(_startingScale.x / parent.localScale.x, _startingScale.y / parent.localScale.y,
                _startingScale.z / parent.localScale.z);
        }
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
    private void IgnoreCollisionWithPaddle(bool ignore){
        Physics.IgnoreCollision(GetComponent<Collider>(), transform.parent.GetComponent<Collider>(), ignore);
    }
    private void RecordVelocity(){
        _recordedVelocity = _rigidbody.velocity.normalized;
    }
    private void RestoreVelocity(){
        _rigidbody.velocity = _recordedVelocity;
        _recordedVelocity = Vector3.zero;
    }
    private void UpdateVelocity(){
        _rigidbody.velocity = _moveSpeed * _rigidbody.velocity.normalized;
    }
}

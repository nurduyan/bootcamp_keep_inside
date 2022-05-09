using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider))]
public class PaddleController : MonoBehaviour{
    [Header("Starting Ball Config")]
    [SerializeField] private List<Ball> _attachedBalls;

    [SerializeField] private float _startingBallLaunchAngleRange;

    [Header("Paddle Config")]
    [SerializeField] private float _minReflectingAngle;

    private BoxCollider _collider;
    private bool _ballAttached = true;
    private bool _glued = false;

    private void Awake(){
        _collider = GetComponent<BoxCollider>();
    }
    private void Start(){
        //Attach starting balls to this paddle
        foreach (Ball ball in _attachedBalls){
            ball.AttachBall();
        }
    }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Space) && _ballAttached){
            DetachBalls();
        }
    }
    public void Glued(float duration){
        StartCoroutine(GlueForDuration(duration));
    }

    IEnumerator GlueForDuration(float duration){
        _glued = true;
        yield return new WaitForSeconds(duration);
        _glued = false;
        if(_ballAttached) DetachBalls();
    }
    private void DetachBalls(){
        //Launch all the balls attached to this paddle off with a random angle from forward vector of this paddle 
        for (int i = 0; i < _attachedBalls.Count; i++){
            Vector3 launchDirection =
                Quaternion.AngleAxis(Random.Range(-_startingBallLaunchAngleRange, _startingBallLaunchAngleRange), transform.up) *
                transform.forward;
            LaunchBall(launchDirection, _attachedBalls[i]);
            _attachedBalls.Remove(_attachedBalls[i]);
        }
        //Disable further inputs
        _ballAttached = false;
    }
    private void LaunchBall(Vector3 launchDirection, Ball ball){
        ball.Launch(launchDirection);
    }
    private void OnCollisionEnter(Collision collision){
        if(!collision.gameObject.CompareTag("Ball")) return;
        if(_glued){
            collision.gameObject.transform.SetParent(transform);
            Ball newBall = collision.gameObject.GetComponent<Ball>();
            newBall.AttachBall();
            _attachedBalls.Add(newBall);
            _ballAttached = true;
        }
        else{
            ContactPoint contactPoint = collision.GetContact(0);
            HandleBallContact(contactPoint, collision.gameObject.GetComponent<Ball>());
        }
    }

    private void HandleBallContact(ContactPoint contactPoint, Ball ball){
        //Find the distance vector from center to contact point of the collision
        Vector3 distanceVectorToCenter = contactPoint.point - transform.position;
        //Project the distance vector onto the local right vector of the paddle to find the distance from the center of paddle
        float distanceToCenter = Vector3.Project(distanceVectorToCenter, transform.right).magnitude;
        //If the angle between distance vector and local right vector is less than 90 it means the ball hit the right side of the paddle otherwise its left
        int leftOrRight = Vector3.Angle(transform.right, distanceVectorToCenter) < 90 ? 1 : -1;
        //Some calculations...
        Vector3 launchDirection =
            //For example our minReflectingAngle is 30. In this case we get 90-30=60.Below when we get a value between 1 and -1 we multiply 60 with it
            //and we get 90 - (1 * 60) for example and it gives us 30 which is minReflectingAngle. What this means is if a ball hits far right side of the
            //paddle we'll get 1 from below and the ball will reflect with 30 degrees. If it hits middle we'll get 0 from the below calculation and we'll get
            //90 - (90 - 30)*0 = 90 which means ball will be reflected 90 degrees and so on...
            Quaternion.AngleAxis((90 - ((90 - _minReflectingAngle) *
                                        //Dividing distance to center to colliders's size at local x axis gives us a value between 0-1 and when we multiply that to leftorright(which is either 1 or -1)
                                        //it gives us a value between -1 and 1 acoording to ball's hit location
                                        leftOrRight * (distanceToCenter / _collider.size.x)))
                , -transform.up) * transform.right;
        LaunchBall(launchDirection, ball);
    }
}

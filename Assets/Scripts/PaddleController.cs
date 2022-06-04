using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider))]
public class PaddleController : MonoBehaviour{
    [Header("Starting Ball Config")]
    [SerializeField] private List<Ball> _attachedBalls;
    [SerializeField] private float _startingBallLaunchAngleRange;
    [SerializeField] private GameObject glueEffect;

    [Header("Paddle Config")]
    [SerializeField] private float _minReflectingAngle;
    [SerializeField] private int _startingOrderNo;
    [SerializeField] private SpawnArea _spawnArea;
    [SerializeField] private bool _leftPaddle;
    [SerializeField] private float _tapDuration;

    private int _detachInputCount = 0;
    private bool _ballAttached = true;
    private bool _gameStarted = false;
    private bool _glued = false;
    private Vector3 _startingScale;
    private float _touchDuration = 0f;

    private void Awake(){
        _startingScale = transform.localScale;
    }
    private void Start(){
        //Attach starting balls to this paddle
        foreach (Ball ball in _attachedBalls){
            ball.AttachBall();
            _spawnArea.AddBall(ball);
        }
    }
    private void Update(){
#if UNITY_EDITOR
        //Keyboard Input
        if(Input.GetKeyDown(KeyCode.Space)){
            if(_gameStarted){
                if(_ballAttached){
                    DetachBalls();
                }
                return;
            }
            _detachInputCount++;
            if(_detachInputCount >= _startingOrderNo && _ballAttached){
                DetachBalls();
            }

            if(_detachInputCount >= 2){
                _spawnArea.StartSpawning();
                _gameStarted = true;
            }
        }
#else
        //Touch Input
        bool touchInput = Input.touchCount > 0;
        if(!touchInput) return;
        Touch touch = Input.GetTouch(0);
        if(EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

        if(touch.phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary){
            _touchDuration += Time.deltaTime;
            return;
        }

        if(touch.phase == TouchPhase.Ended){
            if(_touchDuration <= _tapDuration){
                if(touch.position.x <= Screen.width / 2 && _leftPaddle && _ballAttached){
                    DetachBalls();
                }
                else if(touch.position.x > Screen.width / 2 && !_leftPaddle && _ballAttached){
                    DetachBalls();
                }

                _detachInputCount++;
                if(_detachInputCount >= 2){
                    _spawnArea.StartSpawning();
                    _gameStarted = true;
                }
            }

            _touchDuration = 0f;
        }
#endif
    }
    public void Glued(float duration){
        StartCoroutine(GlueForDuration(duration));
    }
    
    public void SetScale(float changeAmount){
        MoveForScaleIfNearWall(changeAmount);
        Vector3 currentPaddleScale = transform.localScale;
        transform.localScale = new Vector3(changeAmount * currentPaddleScale.x, currentPaddleScale.y, currentPaddleScale.z);
        PreserveBallsScales();
    }
    public void ResetScale(){
        float changeAmount = _startingScale.x / transform.localScale.x;
        MoveForScaleIfNearWall(changeAmount);
        transform.localScale = _startingScale;
        PreserveBallsScales();
    }
    private void MoveForScaleIfNearWall(float changeAmount){
        Vector3 currentPaddleScale = transform.localScale;
        int leftOrRight = 0;
        if(IsNearWall(changeAmount, out leftOrRight)){
            transform.Translate(new Vector3(-leftOrRight * (changeAmount * currentPaddleScale.x - currentPaddleScale.x), 0f, 0f));
        }
    }
    private bool IsNearWall(float changeAmount, out int leftOrRight){
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, changeAmount * transform.localScale.x, 1 << 6);
        if(overlappedColliders.Length == 0){
            leftOrRight = 0;
            return false;
        }
        Vector3 perp = Vector3.Cross(transform.forward, overlappedColliders[0].transform.position - transform.position);
        leftOrRight = System.Math.Sign(Vector3.Dot(perp, transform.up));
        return true;
    }
    private void PreserveBallsScales(){
        for (int i = 0; i < _attachedBalls.Count; i++){
            Vector3 attachedBallScale = _attachedBalls[i].transform.localScale;
            _attachedBalls[i].transform.localScale = new Vector3(attachedBallScale.y * transform.localScale.y / transform.localScale.x,
                attachedBallScale.y, attachedBallScale.z);
        }
    }
    
    IEnumerator GlueForDuration(float duration){
        _glued = true;
        glueEffect.SetActive(true);
        yield return new WaitForSeconds(duration);
        _glued = false;
        glueEffect.SetActive(false);
        if(_ballAttached) DetachBalls();
    }
    private void DetachBalls(){
        //Launch all the balls attached to this paddle off with a random angle from forward vector of this paddle 
        for (int i = 0; i < _attachedBalls.Count; i++){
            Vector3 launchDirection =
                Quaternion.AngleAxis(Random.Range(-_startingBallLaunchAngleRange, _startingBallLaunchAngleRange), transform.up) *
                transform.forward;
            LaunchBall(launchDirection, _attachedBalls[i], true);
        }
        _attachedBalls.Clear();
        //Disable further inputs
        _ballAttached = false;
        if(!_gameStarted){
            FindObjectOfType<GameflowManager>().StartTimerBy(_spawnArea.gameObject);
        }
    }
    private void LaunchBall(Vector3 launchDirection, Ball ball, bool defaultSpeed){
        ball.Launch(launchDirection, defaultSpeed);
    }
    private void OnCollisionEnter(Collision collision){
        if(!collision.gameObject.CompareTag("Ball")) return;
        float angle = Math.Abs(Vector3.Angle(collision.contacts[0].normal, transform.forward));
        if(angle < (90 - _minReflectingAngle)) return;
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
                                        leftOrRight * (distanceToCenter / transform.localScale.x)))
                , -transform.up) * transform.right;
        LaunchBall(launchDirection, ball, false);
    }
}

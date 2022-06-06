using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class Move : MonoBehaviour{
    [Header("Config")]
    [SerializeField] private float _startingSpeed;

    private Rigidbody _rigidbody;
    private float _movementInput = 0f;
    private float _moveSpeed;
    private bool _controllable = true;

    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
        _moveSpeed = _startingSpeed;
    }


    private void Update(){
        if(Time.timeScale > 0){
#if UNITY_EDITOR
            _movementInput = Input.GetAxisRaw("Horizontal");
#else
        //Touch Input
        if(Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            if(!EventSystem.current.IsPointerOverGameObject(touch.fingerId)){
                if(touch.position.x <= Screen.width / 2){
                    _movementInput = -1;
                }
                else{
                    _movementInput = 1;
                }
            }
        }
        else{
            _movementInput = 0;
        }
#endif
        }
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
    public void EnableControls(){
        _controllable = true;
    }
    public void DisableControls(){
        _controllable = false;
    }
}

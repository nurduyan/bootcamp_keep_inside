using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSizePickup : MonoBehaviour, IPickup{

    [SerializeField] private float _sizeChange;
    [SerializeField] private float _pickupDuration;
    [SerializeField] private float _timeToDestroyWithoutPickup;

    private Vector3 _ballStartingScale;
    private Ball _pickedupBall = null; //Bu pickupı triggerlayan top
    private SpawnArea _spawnArea; //Bulunduğumuz bölge
    private Coroutine _destroyCoroutine;

    private void Start(){
        _destroyCoroutine = StartCoroutine(DestroyAfterDuration());
    }
    //Interface methodu. Bu pickup instantiate edildikten sonra instantiate eden SpawnArea nesnesi tarafından çağrılır
    public void SetSpawnArea(SpawnArea spawnArea){
        _spawnArea = spawnArea;
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ball")){
            StopCoroutine(_destroyCoroutine);
            _pickedupBall = other.GetComponent<Ball>();
            _ballStartingScale = _pickedupBall.transform.localScale;
            StartCoroutine(ChangeSizeForDuration());
        }
    }

    //Topun boyutunu belirtilen değer oranında arttırır veya azaltır. Sonrasında ilk haline getirir
    IEnumerator ChangeSizeForDuration(){
        _pickedupBall.ChangeScale(_sizeChange);
        Hide();
        yield return new WaitForSeconds(_pickupDuration);
        _pickedupBall.ResetScale();
        Destroy(gameObject);
    }

    IEnumerator DestroyAfterDuration(){
        yield return new WaitForSeconds(_timeToDestroyWithoutPickup);
        Destroy(gameObject);
    }
    private void Hide(){
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}

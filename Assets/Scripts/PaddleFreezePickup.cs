using System.Collections;
using UnityEngine;

public class PaddleFreezePickup : MonoBehaviour, IPickup{
    [SerializeField] private float _pickupDuration;
    [SerializeField] private float _timeToDestroyWithoutPickup;

    private SpawnArea _spawnArea;//Bulunduğumuz bölge
    private Move _paddleMove;//Bulunduğumuz bölgedeki paddle üzerindeki Move componentı
    private Coroutine _destroyCoroutine;
    
    private void Start(){
        _destroyCoroutine = StartCoroutine(DestroyAfterDuration());
        _paddleMove = _spawnArea.GetAreaPaddle().GetComponent<Move>();
    }
    //Interface methodu. Bu pickup instantiate edildikten sonra instantiate eden SpawnArea nesnesi tarafından çağrılır
    public void SetSpawnArea(SpawnArea spawnArea){
        _spawnArea = spawnArea;
    }
    
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ball")){
            StopCoroutine(_destroyCoroutine);
            StartCoroutine(FreezeForDuration());
        }
    }
    
    //Belirtilen süre boyunca paddleın hızını 0 yapar ve sonrasında eski haline getirir
    IEnumerator FreezeForDuration(){
        _paddleMove.ChangeSpeed(0);
        Hide();
        yield return new WaitForSeconds(_pickupDuration);
        _paddleMove.ResetSpeed();
        Destroy(gameObject);
    }
    IEnumerator DestroyAfterDuration(){
        yield return new WaitForSeconds(_timeToDestroyWithoutPickup);
        Destroy(gameObject);
    }
    private void Hide(){
        GetComponent<MeshRenderer>().enabled = false;
        foreach (var renderer in GetComponentsInChildren<Renderer>()){
            renderer.enabled = false;
        }
        GetComponent<Collider>().enabled = false;
    }
}

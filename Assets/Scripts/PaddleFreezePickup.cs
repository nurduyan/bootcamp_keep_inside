using System.Collections;
using UnityEngine;

public class PaddleFreezePickup : MonoBehaviour, IPickup{
    [SerializeField] private float _pickupDuration;
    [SerializeField] private float _timeToDestroyWithoutPickup;

    private SpawnArea _spawnArea;//Bulunduğumuz bölge
    private Move _paddleMove;//Bulunduğumuz bölgedeki paddle üzerindeki Move componentı
    private Coroutine _destroyCoroutine;
    private PaddleController _paddle;
    private void Start(){
        _destroyCoroutine = StartCoroutine(DestroyAfterDuration());
        _paddleMove = _spawnArea.GetAreaPaddle().GetComponent<Move>();
        _paddle = _spawnArea.GetAreaPaddle();
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
        _paddle.transform.GetChild(1).gameObject.SetActive(true);
        Hide();
        yield return new WaitForSeconds(_pickupDuration);
        _paddleMove.ResetSpeed();
        _paddle.transform.GetChild(1).gameObject.SetActive(false);
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

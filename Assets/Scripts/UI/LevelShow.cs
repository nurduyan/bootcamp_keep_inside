using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LevelShow : MonoBehaviour{
    private void Start(){
        GetComponent<TextMeshProUGUI>().text = SceneManager.GetActiveScene().name;
    }
}
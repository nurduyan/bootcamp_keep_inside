using UnityEngine;
using UnityEngine.SceneManagement;

public class TransparentWall : MonoBehaviour{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            if(AdsManager.Instance != null){
                AdsManager.Instance.IncLevelPassOrDeath();
            }
            SceneManager.LoadScene("RetryGameScene");
        }
    }
}

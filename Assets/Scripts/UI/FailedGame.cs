using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailedGame : MonoBehaviour
{

    
    public void ReturnButton2()
    {
        SceneManager.LoadScene(0);

    }
    public void StartGameButton2()
    {
        SceneManager.LoadScene(2);
    }

}

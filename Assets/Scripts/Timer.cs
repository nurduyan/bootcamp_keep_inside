using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timer;
    public TextMeshProUGUI timer_text;
    void Start()
    {
        timer_text.text = "= " + (int)timer;
    }

    
    void Update()
    {
            timer -= Time.deltaTime;
            timer_text.text = "= " + (int) timer;
            if (timer <= 0)
            {
                timer_text.text = "Time is up !";
                SceneManager.LoadScene("RetryGameScene");

            }
            else
            {
                timer_text.text = "= " + (int) timer;
            }
        

    }
}

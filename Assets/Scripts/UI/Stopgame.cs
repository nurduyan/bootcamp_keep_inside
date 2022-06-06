using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopgame : MonoBehaviour
{
    public bool oyundurumu = false;
    
    
    public void oyunudurdur()
    {
       if(oyundurumu == true)
        {
            Time.timeScale = 1f;
            oyundurumu = false;
        }
        else
        {
            Time.timeScale = 0f;
            oyundurumu = true;
        }
        
    }

}
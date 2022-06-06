using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour{
    private void Start(){
        if(!DataPersistenceManager.Instance.HasSaveData()){
            gameObject.SetActive(false);
        }
    }
}

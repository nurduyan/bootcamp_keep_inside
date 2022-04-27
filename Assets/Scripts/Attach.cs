using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach : MonoBehaviour{
    [SerializeField] private Transform attachPoint;
    private void Awake(){
        GetComponent<Rigidbody>().isKinematic = true;
    }
}

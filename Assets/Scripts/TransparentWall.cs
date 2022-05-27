using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransparentWall : MonoBehaviour{
    [SerializeField] private SpawnArea _spawnArea;
    public int numberOfBalls;
    private void Start()
    {
          numberOfBalls = GameObject.FindGameObjectsWithTag("Ball").Length +
                               GameObject.Find("Paddle").transform.GetChild(0).tag.Length +
                               GameObject.Find("Paddle1").transform.GetChild(0).tag.Length;
    }
    private void Update()
    {
        numberOfBalls = GameObject.FindGameObjectsWithTag("Ball").Length;

        if (numberOfBalls == 0)
        {
            SceneManager.LoadScene("RetryGameScene");
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            _spawnArea.RemoveBall(other.gameObject.GetComponent<Ball>());
            Destroy(other.gameObject);
            numberOfBalls--;
        }
    }
}

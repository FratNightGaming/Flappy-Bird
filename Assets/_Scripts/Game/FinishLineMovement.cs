using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineMovement : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 startPos;

    private void OnEnable()
    {
        GameManager.stateChange += ResetPositionGameOver;
    }

    private void OnDisable()
    {
        GameManager.stateChange -= ResetPositionGameOver;
    }

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        //if (GameManager.instance.state == GameManager.GameState.Victory)
        {
            transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }


        if (transform.position.x <= -10f)
        {
            transform.position += Vector3.zero;
        }
    }

    public void ResetPositionVictory()
    {
        transform.position = startPos;
        this.gameObject.SetActive(false);
    }

    public void ResetPositionGameOver(GameManager.GameState state)
    {
        if(state == GameManager.GameState.GameOver)
        {
            transform.position = startPos;
            this.gameObject.SetActive(false);
        }
    }
}

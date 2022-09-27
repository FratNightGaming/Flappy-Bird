using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    public float moveSpeed;
    private float groundWidth;
    private BoxCollider2D box;

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        //Debug.Log(groundWidth);

    }

    void Update()
    {
        MoveLeft();
    }

    private void MoveLeft()
    {

        groundWidth = box.size.x * 12;

        transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);

        if (transform.position.x <= -groundWidth)
        {
            transform.position = new Vector3(groundWidth, transform.position.y, transform.position.z);
        }
    }
}

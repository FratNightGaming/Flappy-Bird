using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    public GameObject pipe;

    public Animator camShake;

    private Vector2 offScreen;

    private float pipeWidth;

    public float moveSpeed;

    private void OnEnable()
    {
        //GameManager.stateChange += DestroyPipes;

    }

    private void OnDisable()
    {
        //GameManager.stateChange += DestroyPipes;

    }

    void Start()
    {
        offScreen = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        pipeWidth = pipe.GetComponent<BoxCollider2D>().size.x;
        offScreen.x -= pipeWidth;
        //Debug.Log(offScreen);
    }

    void Update()
    {
        Movement();

        StartCoroutine("DestroyPipes");
    }

    public void Movement()
    {
        if (GameManager.instance.state != GameManager.GameState.GameOver)
        {
            transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }

        //if (transform.position.x <= offScreen.x)
        if (transform.position.x <= -12f)
        {
            Destroy(this.gameObject);
        }
    }

    public IEnumerator DestroyPipes()
    {
        if(GameManager.instance.state == GameManager.GameState.GameOver)
        {
            yield return new WaitForSeconds(1f);

            PipeMovement[] pipes = FindObjectsOfType<PipeMovement>();

            foreach (PipeMovement pipe in pipes)
            {
                Destroy(pipe.gameObject);
            }
        }
    }
}

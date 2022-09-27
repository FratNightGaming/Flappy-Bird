using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    public GameObject player;
    private Vector3 startPos;


    [Header("Zoom Variables")]
    public float zoomTimer;
    public float startZoom;
    public float endZoom;
    public Vector3 endPos;

    private void OnEnable()
    {
        GameManager.stateChange += ZoomOut;
    }

    private void OnDisable()
    {
        GameManager.stateChange -= ZoomOut;
    }

    void Start()
    {
        startPos = transform.position;
        cam = GetComponent<Camera>();
        startZoom = cam.orthographicSize;
    }

    void Update()
    {
        ZoomIn();
    }
    

    public void ZoomIn()
    {
        if (GameManager.instance.score == GameManager.maxScore && GameManager.instance.state != GameManager.GameState.GameOver)
        {
            zoomTimer += Time.deltaTime;
            float zoomMax = 5f;
            if (zoomTimer >= zoomMax)
            {
                zoomTimer = zoomMax;
            }

            cam.orthographicSize = Mathf.Lerp(startZoom, endZoom, zoomTimer / zoomMax);
            //transform.position = Vector3.Lerp(transform.position, endPos, zoomTimer / 3f);
        }
    }

    public void ZoomOut (GameManager.GameState state)
    {
        if (state == GameManager.GameState.Playing)
        {
            cam.orthographicSize = startZoom;
            zoomTimer = 0f;
            cam.transform.position = startPos;
        }

        if (state == GameManager.GameState.GameOver)
        {
            cam.orthographicSize = cam.orthographicSize;
            zoomTimer = 0f;
            cam.transform.position = startPos;
        }
    }
}

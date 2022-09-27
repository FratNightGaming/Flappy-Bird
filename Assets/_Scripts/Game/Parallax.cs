using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private MeshRenderer mr;
    private BoxCollider2D box;
    [SerializeField]
    [Range(.01f, .5f)]
    private float parallaxspeed;
    private Vector2 endOffset;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        box = GetComponent<BoxCollider2D>();
        endOffset = Vector2.zero;
    }

    void Update()
    {
        MoveGround();
        SetParallaxSpeed();
    }

    public void MoveGround()
    {
        mr.material.mainTextureOffset += new Vector2(parallaxspeed * Time.deltaTime, 0);
    }

    public void SetParallaxSpeed()
    {
        float parallaxTimer = 0f;

        if (GameManager.instance.score >= GameManager.maxScore
            && GameManager.instance.state != GameManager.GameState.Victory
            && GameManager.instance.state != GameManager.GameState.GameOver)
        {
            parallaxTimer += Time.deltaTime;
            if (parallaxTimer >= 3f)
            {
                parallaxTimer = 3f;
            }

            parallaxspeed = Mathf.Lerp(parallaxspeed, .01f, parallaxTimer/3f);
        }

        else if (GameManager.instance.state == GameManager.GameState.Victory)
        {
            parallaxspeed = Mathf.Lerp(parallaxspeed, 0f, 1);
            //mr.material.mainTextureOffset = endOffset;
            parallaxTimer = 0f;
        }

        else if (GameManager.instance.state == GameManager.GameState.GameOver)
        {
            parallaxspeed = 0f;
            parallaxTimer = 0f;
            //mr.material.mainTextureOffset = endOffset;
        }

        else
        {
            parallaxspeed = .15f;
        }
    }
}

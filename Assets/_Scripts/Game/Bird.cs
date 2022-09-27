using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird : MonoBehaviour
{
    public float victoryTimer = 0f;

    [Header("Physics")]
    Rigidbody2D rb;
    public float jumpHeight;
    public float gravityTimer = 0f;
    public float rotateFadeInTimer = 0f;
    public float gravityScale;
    public bool normalGravity;

    [Header("Rotation")]
    public float rotateTimer = 0f;
    public float rotateMax = .5f;
    public float maxAngle = 40f;
    public float minAngle = -40f;
    public float upDuration;
    public float downDuration;

    public GameObject finishLine;

    [Header("Animations")]
    public Animator animator;
    public Animator cameraAnimator;
    public Animator flying;


    private Vector3 startPos;

    public void OnEnable()
    {
        GameManager.stateChange += GameOverAnimations;
        GameManager.stateChange += ResetBirdPhysics;
        //GameManager.stateChange += VictoryBird;

    }

    public void OnDisable()
    {
        GameManager.stateChange -= GameOverAnimations;
        GameManager.stateChange -= ResetBirdPhysics;
        //GameManager.stateChange -= VictoryBird;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        startPos = transform.position;
        gravityScale = rb.gravityScale;
    }

    void Update()
    {
        if(GameManager.instance.state == GameManager.GameState.Playing)
        {
            Jump();
            BirdRotation();
            GravityFadeIn();
            RotationFadeIn();
        }

        MaxScoreCheck();
        VictoryBird();
    }

    private void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {

            AudioManager.audiomanager.Play("Flap");
            //Debug.Log(rb.velocity.y);
            if (GameManager.instance.score < GameManager.maxScore)
            {
                rb.velocity = Vector2.zero;
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }

            else
            {
                //jumpHeight = 3f;
                jumpHeight -= Time.deltaTime * 25;
                if (jumpHeight <= 3)
                {
                    jumpHeight = 3;
                }

                rb.velocity = Vector2.zero;
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }
        }
    }

    private void BirdRotation()
    {
        if (normalGravity == true)
        {
            Quaternion noseUp = Quaternion.Euler(0, 0, maxAngle);
            Quaternion noseDown = Quaternion.Euler(0, 0, minAngle);

            if (rb.velocity.y > 0)
            {
                rotateTimer = 0;
                rotateTimer += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, noseUp, rotateTimer / upDuration);


                if (rotateTimer > rotateMax)
                {
                    rotateTimer = rotateMax;
                }
            }

            else if (rb.velocity.y < -1f)
            {
                rotateTimer = 0;
                rotateTimer += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, noseDown, rotateTimer / downDuration);


                if (rotateTimer > rotateMax)
                {
                    rotateTimer = rotateMax;
                }
            }
        }
            
    }

    private void GravityFadeIn()
    {
        float gravityBegin = .01f;
        float gravityEnd = 1f;

        gravityTimer += Time.deltaTime;

        if (gravityTimer <= 1.5f)
        {
            rb.gravityScale = Mathf.Lerp(gravityBegin, gravityEnd, gravityTimer / 1.5f);
        }

        else
        {
            rb.gravityScale = 1;
        }
    }

    public void RotationFadeIn()
    {
        rotateFadeInTimer += Time.deltaTime;

        Quaternion noseUp = Quaternion.Euler(0, 0, 20f);
        Quaternion noseDown = Quaternion.Euler(0, 0, -5f);


        if (rotateFadeInTimer <= 1.5)
        {
            if (rb.velocity.y > 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, noseUp, rotateFadeInTimer / 1.5f);
            }

            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, noseDown, rotateFadeInTimer / 1.5f);
            }
        }

        else
        {
            normalGravity = true;
        }
    
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Score"))
        {
            GameManager.instance.AddPoint();
        }

        if (collision.gameObject.name == "Finish Line")
        {
            GameManager.instance.UpdateGameState(GameManager.GameState.Victory);
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.instance.state == GameManager.GameState.Playing)
        {
            if (collision.gameObject.CompareTag("Pipe"))
            {
                AudioManager.audiomanager.Play("Hit");
                flying.SetTrigger("NoFlap");
                GameManager.instance.UpdateGameState(GameManager.GameState.GameOver);
            }

            if (collision.gameObject.CompareTag("Ground"))
            {
                AudioManager.audiomanager.Play("Hit");
                flying.SetTrigger("NoFlap");
                GameManager.instance.UpdateGameState(GameManager.GameState.GameOver);
            }
        }

        if (GameManager.instance.state == GameManager.GameState.Victory)

                if (collision.gameObject.CompareTag("Ground"))
                {
                    flying.SetTrigger("NoFlap");
                }
    }

    public void MaxScoreCheck()
    {
        if (GameManager.instance.state == GameManager.GameState.Playing && GameManager.instance.score >= GameManager.maxScore)

        {
            finishLine.SetActive(true);
        }
    }
    public void GameOverAnimations(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver)
        {
            animator.Play("White_Out");
            cameraAnimator.SetTrigger("shake");
            animator.SetTrigger("newGame");
            cameraAnimator.SetTrigger("unshake");
        }
    }

    public void ResetPosition()
    {
        transform.position = startPos;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void VictoryBird()
    {
        if (GameManager.instance.state == GameManager.GameState.Victory)

        {
            float victoryTimerMax = 3f;
            victoryTimer += Time.deltaTime;

            float yVel = Mathf.Lerp(rb.velocity.y, -1f, victoryTimer/victoryTimerMax);
            rb.velocity = new Vector2(rb.velocity.x, yVel);
            //rb.gravityScale = Mathf.Lerp(rb.gravityScale, 0, 1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0), victoryTimer /victoryTimerMax);
        }
    }

    public void ResetBirdPhysics(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver)
        {
            gravityTimer = 0f;
            rotateFadeInTimer = 0f;
            jumpHeight = 6f;
            rotateFadeInTimer = 0f;
            gravityTimer = 0f;
            normalGravity = false;
        }

        if (state == GameManager.GameState.Victory)
        {
            gravityTimer = 0f;
            rotateFadeInTimer = 0f;
            jumpHeight = 6f;
            rotateFadeInTimer = 0f;
            gravityTimer = 0f;
            normalGravity = false;
        }

        if (state == GameManager.GameState.Playing)
        {
            victoryTimer = 0f;
            jumpHeight = 6f;
        }

    }

    public void SetFlapTrigger()
    {
        flying.SetTrigger("Flap");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static event Action<GameState> stateChange;

    public enum GameState
    {
        Pregame, Playing, GameOver, Pause, Victory
    }

    [Header("GameStates")]
    public GameState state;

    [Header("Player Stats")]
    public int score = 0;
    public int highScore;
    public bool gameOver;
    public bool isPaused;
    public bool newRecord;
    public int drawScore;
    public static int maxScore = 30;

    [Header("UI Elements")]
    public GameObject gameOverScreen;
    public GameObject bird;
    public GameObject playButton;
    public GameObject getReady;
    public GameObject newHighScore;
    public GameObject finishLine;
    public GameObject victoryPanel;

    [Header("Text Elements")]
    public TextMeshProUGUI scoreTextMesh;
    public Text endGameScoreText;
    public Text highScoreText;
    public Text victoryScore;
    public Text victoryHighScore;

    public Image[] medals;
    public Image medal;
    public Image VictoryMedal;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        gameOver = false;
        gameOverScreen.SetActive(false);
        newHighScore.SetActive(false);
        getReady.SetActive(true);
        finishLine.SetActive(false);
        victoryPanel.SetActive(false);
        UpdateGameState(GameState.Pregame);  
    }

    void Update()
    {
        UpdateScoreTexts();
        StartCoroutine("Pause");
        StartCoroutine("Unpause");

    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        if (stateChange != null)
       {
           stateChange(newState);
       }

        switch (newState)
        {
            case GameState.Pregame:
                PregameState();
                break;
            case GameState.Playing:
                StartCoroutine("PlayingState");
                break;
            case GameState.GameOver:
                StartCoroutine("GameOverState");
                break;
            case GameState.Pause:
                PauseState();
                break;
            case GameState.Victory:
                StartCoroutine("VictoryState");
                break;
            default:
                break;
        }
    }

    public IEnumerator VictoryState()
    {
        yield return new WaitForSeconds(1.5f);

        victoryPanel.SetActive(true);
        scoreTextMesh.gameObject.SetActive(false);
        gameOverScreen.GetComponent<Animator>().SetTrigger("Splash");
        victoryPanel.GetComponent<Animator>().SetTrigger("GoldSplash");
        newHighScore.SetActive(true);
    }

    public void PregameState()
    {
        ResetScore();
        bird.GetComponent<Rigidbody2D>().gravityScale = 0f;
        scoreTextMesh.gameObject.SetActive(false);
        newHighScore.SetActive(false);
        getReady.SetActive(true);
        finishLine.SetActive(false);
        playButton.SetActive(true);
    }

    public IEnumerator PlayingState()
    {
        scoreTextMesh.gameObject.SetActive(true);
        playButton.SetActive(false);
        getReady.SetActive(false);
        medal.enabled = false;
        newHighScore.SetActive(false);
        SetTimeScale(1f);
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateGameState(GameState.Pause);
        }*/
        yield return new WaitForSeconds(.10f);
    }

    public IEnumerator GameOverState()
    {
        bird.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        bird.GetComponent<Rigidbody2D>().gravityScale = 0f;
        gameOverScreen.SetActive(true);
        AudioManager.audiomanager.Play("Die");
        scoreTextMesh.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        bird.transform.position = new Vector3(-5, 1, 0);
        bird.transform.rotation = Quaternion.Euler(0, 0, 0);
        bird.GetComponent<Rigidbody2D>().gravityScale = 0f;
        bird.GetComponent<Animator>().SetTrigger("Flap");
        gameOverScreen.GetComponent<Animator>().SetTrigger("Splash");
        finishLine.SetActive(false);
        AssignMedalSprite();
        if (newRecord)
        {
            newHighScore.SetActive(true);
        }
    }

    public void OKButton()
    {
        ResetScore();
        gameOverScreen.SetActive(false);
        scoreTextMesh.gameObject.SetActive(true);
        newHighScore.SetActive(false);
        UpdateGameState(GameState.Playing);
        AudioManager.audiomanager.Play("Transition");
    }

    public void StartButton()
    {
        ResetScore();
        playButton.SetActive(false);
        victoryPanel.SetActive(false);
        UpdateGameState(GameState.Playing);
    }

    public void PauseState()
    {
        SetTimeScale(0f);
        playButton.SetActive(true);
    }

    public IEnumerator Pause()
    {
        if (state == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                yield return new WaitForSeconds(.04f);
                UpdateGameState(GameState.Pause);
            }
        }
    }

    public IEnumerator Unpause()
    {
        if (state == GameState.Pause)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SetTimeScale(1f);
                yield return new WaitForSeconds(.04f);
                UpdateGameState(GameState.Playing);
            }
        }
    }

    public void UpdateScoreTexts()
    {
        scoreTextMesh.text = score.ToString();
        endGameScoreText.text = score.ToString();
        victoryScore.text = score.ToString();
        victoryHighScore.text = score.ToString();

        if (highScore <= score && score > 0)
        {
            newRecord = true;
            highScore = score;
        }

        else
        {
            newRecord = false;
        }

        highScoreText.text = highScore.ToString();

    }

    /*public void DrawScore()
    {
        if (drawScore < score)
        {
            scoreTextMesh.text = drawScore.ToString();
            drawScore++;
            Invoke("DrawScore", .04f);
        }
    }*/

    public void AddPoint()
    {
        score++;
        AudioManager.audiomanager.Play("Point");
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        AudioManager.audiomanager.Play("Transition");
    }

    public void AssignMedalSprite()
    {
        if (score < 10)
        {
            medal.enabled = false;
        }

        if (score >= 10 && score < 20)
        {
            medal.enabled = true;
            medal.sprite = medals[0].sprite;
        }

        if (score >= 20 && score < maxScore)
        {
            medal.enabled = true;
            medal.sprite = medals[1].sprite;
        }

        if (score >= maxScore)
        {
            medal.enabled = true;
            medal.sprite = medals[2].sprite;
        }
    }
}

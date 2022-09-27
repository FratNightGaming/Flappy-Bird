using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject[] pipes = new GameObject[3];
    private float randomY;
    public float minY;
    public float maxY;
    //public float spawnTimer;
    public float maxDuration;
    public float startDelay = .5f;
    public float spawnDelay = 1f;
    public bool canStart = true;

    public List<int> spawnedPipes = new List<int>();

    public int levelOne = 9;
    public int levelTwo = 19;
    public int levelThree = 29;

    private void OnEnable()
    {
        GameManager.stateChange += PipeSpawnCheck;
        GameManager.stateChange += ResetPipeList;
    }

    private void OnDisable()
    {
        GameManager.stateChange -= PipeSpawnCheck;
        GameManager.stateChange -= ResetPipeList;
    }

    void Start()
    {
        
    }

    void Update()
    {
        //GameStateCheck();

        //ResetPipeList();
    }

    public void PipeSpawnCheck(GameManager.GameState state)
    {
        switch (GameManager.instance.state)
        {
            case GameManager.GameState.Playing:
                if (canStart == true)
                {
                    StartCoroutine("Spawner");
                }
                break;

            case GameManager.GameState.GameOver:
                {
                    StopCoroutine("Spawner");
                    canStart = true;
                }
                break;

            case GameManager.GameState.Pause:
                {
                    canStart = false;
                }
                break;

            case GameManager.GameState.Victory:
                {
                    StopCoroutine("Spawner");
                    canStart = true;
                }
                break;
        }

    }

    public IEnumerator Spawner()
    {
            yield return new WaitForSeconds(startDelay);

            for (int i = 0; i < GameManager.maxScore; i++)

            if (spawnedPipes.Count <= levelOne)
            {
                spawnedPipes.Add(1);
                minY = -1.66f;
                maxY = 2.67f;
                GameObject pipes1 = Instantiate(pipes[0], transform.position, Quaternion.identity);
                pipes1.transform.position += Vector3.up * Random.Range(minY, maxY);
                //spawnTimer = 0f;
                yield return new WaitForSeconds(spawnDelay);
            }

            else if (spawnedPipes.Count > levelOne && spawnedPipes.Count <= levelTwo)
            {
                spawnedPipes.Add(2);
                minY = -1.93f;
                maxY = 3.12f;
                GameObject pipes2 = Instantiate(pipes[1], transform.position, Quaternion.identity);
                pipes2.transform.position += Vector3.up * Random.Range(minY, maxY);
                //spawnTimer = 0f;
                yield return new WaitForSeconds(spawnDelay);
            }

            else if (spawnedPipes.Count > levelTwo && spawnedPipes.Count <= levelThree)
            {
                spawnedPipes.Add(3);
                minY = -2.11f;
                maxY = 3.1f;
                GameObject pipes3 = Instantiate(pipes[2], transform.position, Quaternion.identity);
                pipes3.transform.position += Vector3.up * Random.Range(minY, maxY);
                yield return new WaitForSeconds(spawnDelay);
            }
    }

    public void DisablePipeSpawner()
    {
        this.gameObject.SetActive(false);
    }

    public void ResetPipeList(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver)
        {
            spawnedPipes.Clear();
        }

        if (state == GameManager.GameState.Victory)
        {
            spawnedPipes.Clear();
        }
    }

    /*public void SpawnPipe()
    {
        //spawnTimer += Time.deltaTime;

        if (spawnTimer >= maxDuration)
        {
        randomY = Random.Range(minY, maxY);
        Instantiate(pipes[0], new Vector3(transform.position.x, randomY, transform.position.z), Quaternion.identity);
        spawnTimer = 0f;
        //SpawnPipe();
        }

    }*/
}

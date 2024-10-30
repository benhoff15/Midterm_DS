using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform[] spawnpoints;
    public GameObject duckPrefab;
    //public SpriteRenderer bg;
    public Color blueColor;
    public Font electronicHighwayFont;

    public GameObject dogMiss, dogHit;
    public SpriteRenderer dogSprite;
    public Sprite[] victorySprites;

    public TextMeshProUGUI roundText;
    int roundNumber = 1;
    int totalTrials = 10;

    public int totalHits;
    int ducksCreated;
    bool isRoundOver;

    public TextMeshProUGUI scoreText, hitsText;
    int score, hits, totalClicks;

    public GameObject bombPrefab;
    private float bombSpawnChance = 0.05f;

    private float roundDuration = 60f;
    private float timeRemaining;
    private bool roundActive = false;
    public AudioSource gunshotSource;


    void Start()
    {
        if (SceneManager.GetActiveScene().name == "HomeScreen")
        {
            return;
        }
        InitializeGame();
    }
    private void InitializeGame()
    {
        StartRound();
        Instance = this;
        StartCoroutine(RunRound());
    }

    void Update()
    {
        if (roundActive)
        {
            UpdateTimer();
        }
        if (Input.GetMouseButtonDown(0))
        {
            gunshotSource.Play();
        }
        if (Input.GetMouseButtonDown(0))
            totalClicks++;

        scoreText.text = score.ToString("000");
        hitsText.text = hits.ToString() + "/" + totalClicks.ToString();
    }

    IEnumerator RunRound()
    {
        float roundDuration = 60f; 
        float spawnInterval = 10f;  
        float elapsedTime = 0f;

        while (elapsedTime < roundDuration)
        {
            StartCoroutine(CreateDucks(5)); 
            yield return new WaitForSeconds(spawnInterval);
            elapsedTime += spawnInterval;
        }

        isRoundOver = true;
        Debug.Log("Round Over");
    }
    private void StartRound()
    {
        timeRemaining = roundDuration;
        roundActive = true;
    }
    private void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            EndRound();
        }
    }
    void OnGUI()
    {
        if (roundActive)
        {
            GUI.depth = 5;

            GUIStyle timerStyle = new GUIStyle();
            timerStyle.fontSize = 50; 
            timerStyle.normal.textColor = Color.white; 

            if (electronicHighwayFont != null)
            {
                timerStyle.font = electronicHighwayFont;
            }

            float xPosition = Screen.width / 2 - 350; 
            float yPosition = Screen.height - 111; 

            GUI.Label(new Rect(xPosition, yPosition, 200, 50), "Time Remaining: " + Mathf.Ceil(timeRemaining).ToString(), timerStyle);
        }
    }
    public void CallCreateDucks()
    {
        StartCoroutine(CreateDucks(10));
    }
    IEnumerator CreateDucks(int _count)
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < _count; i++)
        {
            Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
            Instantiate(duckPrefab, spawnpoint.position, Quaternion.identity);

            if (Random.value < bombSpawnChance)
            {
                Vector3 bombPosition = new Vector3(10.0f, Random.Range(-3.0f, 3.0f), 0); 
                GameObject bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity);
                bomb.GetComponent<Bomb>().speed = 10.0f; 
            }

            yield return new WaitForSeconds(0.5f); 
        }
        StartCoroutine(TimeUp());
    }


    void SpawnEntity()
    {
        foreach (Transform spawnpoint in spawnpoints)
        {
            if (Random.value < bombSpawnChance)
            {
                Instantiate(bombPrefab, spawnpoint.position, Quaternion.identity);
            }
            else
            {
                Instantiate(duckPrefab, spawnpoint.position, Quaternion.identity);
            }
        }
    }
    IEnumerator TimeUp()
    {
        yield return new WaitForSeconds(60f);  
        Duck[] ducks = FindObjectsOfType<Duck>();
        for (int i = 0; i < ducks.Length; i++)
        {
            ducks[i].Timeup();
        }
        yield return new WaitForSeconds(0.25f);
        if (!isRoundOver)
            StartCoroutine(RoundOver());
    }
    public void HitDuck()
    {
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        totalHits++;
        score += 10;
        hits++;
        //bg.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        //bg.color = blueColor;
        ducksCreated--;

        if (ducksCreated <= 0)
        {
            if (!isRoundOver)
            {
                StopCoroutine(TimeUp());
                StartCoroutine(RoundOver());
            }
        }
    }
    IEnumerator RoundOver()
    {
        isRoundOver = true;
        yield return new WaitForSeconds(1f);

        if (ducksCreated <= 0)
        {
            dogHit.SetActive(true);
            dogSprite.sprite = victorySprites[0];
        }
        else if (ducksCreated == 1)
        {
            dogHit.SetActive(true);
            dogSprite.sprite = victorySprites[1];
        }
        else
        {
            dogMiss.SetActive(true);
        }
        yield return new WaitForSeconds(2f);
        dogHit.SetActive(false);
        dogMiss.SetActive(false);
        CallCreateDucks();
        isRoundOver = false;
    }
    private void EndRound()
    {
        roundActive = false;
        Debug.Log("Round over!");
    }

}
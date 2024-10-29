using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform[] spawnpoints;
    public GameObject duckPrefab;
    //public SpriteRenderer bg;
    public Color blueColor;

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


    void Start()
    {
        Instance = this;
        StartCoroutine(RunRound());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            totalClicks++;

        scoreText.text = score.ToString("000");
        hitsText.text = hits.ToString() + "/" + totalClicks.ToString();
    }
   
    IEnumerator RunRound()
    {
        float roundDuration = 60f;  // 60 seconds for each round
        float spawnInterval = 10f;   // Spawn every 10 seconds
        float elapsedTime = 0f;

        while (elapsedTime < roundDuration)
        {
            StartCoroutine(CreateDucks(5));  // Spawn 5 ducks every 10 seconds
            yield return new WaitForSeconds(spawnInterval);
            elapsedTime += spawnInterval;
        }

        // End of round actions (if needed)
        isRoundOver = true;
        Debug.Log("Round Over");
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
            // Spawn a duck at a random spawn point
            Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
            Instantiate(duckPrefab, spawnpoint.position, Quaternion.identity);

            // Chance to spawn a bomb at the far right with random height
            if (Random.value < bombSpawnChance)
            {
                Vector3 bombPosition = new Vector3(10.0f, Random.Range(-3.0f, 3.0f), 0); // Far right, random vertical position
                GameObject bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity);
                bomb.GetComponent<Bomb>().speed = 10.0f; // Set speed to move leftward, adjust as needed
            }

            yield return new WaitForSeconds(0.5f); // Delay between duck spawns
        }
        StartCoroutine(TimeUp());
    }


    void SpawnEntity()
    {
        foreach (Transform spawnpoint in spawnpoints)
        {
            // Decide whether to spawn a duck or a rare bomb
            if (Random.value < bombSpawnChance)
            {
                // Spawn a bomb
                Instantiate(bombPrefab, spawnpoint.position, Quaternion.identity);
            }
            else
            {
                // Spawn a duck
                Instantiate(duckPrefab, spawnpoint.position, Quaternion.identity);
            }
        }
    }
    IEnumerator TimeUp()
    {
        yield return new WaitForSeconds(60f);  // Wait for 60 seconds instead of 10
        Duck[] ducks = FindObjectsOfType<Duck>();
        for (int i = 0; i < ducks.Length; i++)
        {
            ducks[i].Timeup();
        }
        //bg.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        //bg.color = blueColor;
        if (!isRoundOver)
            StartCoroutine(RoundOver());
    }
    public void HitDuck()
    {
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        //totalClicks++;
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
}
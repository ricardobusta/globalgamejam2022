using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text timerLabel;
    [SerializeField] private TMP_Text scoreLabel;
    
    [SerializeField] private int warmSpawnAmount = 10;
    [SerializeField] private float maxSpawnDelay = 5;
    [SerializeField] private float minSpawnDelay = 0.5f;
    [SerializeField] private float spawnDelayDecrease = 0.01f;
    [SerializeField] private float maxSpawnAmount = 50;
    [SerializeField] private float minSpawnAmount = 5;
    [SerializeField] private float spawnAmountIncrease = 0.01f;
    [SerializeField] private int maxEnemyCount = 300;
    [SerializeField] private float matchDuration = 120;

    [NonSerialized] public readonly List<Enemy> activeEnemiesList = new List<Enemy>();
    [NonSerialized] private readonly Stack<Enemy> enemiesPool = new Stack<Enemy>();

    private Spawner[] _spawners;
    public float _spawnDelay;
    public float _spawnAmount;
    public bool _gameOver;

    private int _score;
    private float _timer;

    private void Start()
    {
        _spawners = FindObjectsOfType<Spawner>();

        for (var i = 0; i < warmSpawnAmount; i++)
        {
            SpawnRat();
        }

        StartCoroutine(SpawnRoutine());

        _spawnDelay = maxSpawnDelay;
        _spawnAmount = minSpawnAmount;

        _score = 0;
        _timer = matchDuration;
    }

    private void Update()
    {
        if (_gameOver)
        {
            return;
        }
        
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            timerLabel.text = Mathf.Floor(_timer).ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            PlayerPrefs.SetInt("SCORE", _score);
            SceneManager.LoadScene("Scenes/ScoreScreen");
            _gameOver = true;
        }
        
        if (_spawnDelay <= minSpawnDelay)
        {
            _spawnDelay = minSpawnDelay;
        }
        else
        {
            _spawnDelay -= spawnDelayDecrease * Time.deltaTime;
        }

        if (_spawnAmount >= maxSpawnAmount)
        {
            _spawnAmount = maxSpawnAmount;
        }
        else
        {
            _spawnAmount += spawnAmountIncrease * Time.deltaTime;
        }

        var playerPos = player.transform.position;
        foreach (var enemy in activeEnemiesList)
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.SetTarget(playerPos);
            }
        }
    }

    private void SpawnRat()
    {
        if (activeEnemiesList.Count >= maxEnemyCount)
        {
            return;
        }
        
        var pos = GetSpawnPosition();
        var isLight = Random.Range(0, 2) % 2 == 1;

        var enemy = enemiesPool.Count > 0
            ? enemiesPool.Pop()
            : Instantiate(enemyPrefab);

        enemy.Init(isLight, pos, Quaternion.identity, e =>
        {
            activeEnemiesList.Remove(e);
            enemiesPool.Push(e);
            _score++;
            scoreLabel.text = _score.ToString(CultureInfo.InvariantCulture);
        });
        activeEnemiesList.Add(enemy);
    }

    private Vector3 GetSpawnPosition()
    {
        return _spawners[Random.Range(0, _spawners.Length)].spawnPos.position;
    }

    private IEnumerator SpawnRoutine()
    {
        while (!_gameOver)
        {
            yield return new WaitForSeconds(_spawnDelay);

            var amount = Mathf.Floor(_spawnAmount);
            Debug.Log($"Spawning {amount} rats");
            for (var i = 0; i < amount; i++)
            {
                SpawnRat();
            }
        }
    } 
}
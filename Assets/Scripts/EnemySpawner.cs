using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Player player;
    [SerializeField] private int warmSpawnAmount;
    [SerializeField] private float maxSpawnDelay;
    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float spawnDelayDecrease;
    [SerializeField] private float maxSpawnAmount;
    [SerializeField] private float minSpawnAmount;
    [SerializeField] private float spawnAmountIncrease;
    [SerializeField] private int maxEnemyCount;

    [NonSerialized] public readonly List<Enemy> activeEnemiesList = new List<Enemy>();
    [NonSerialized] private readonly Stack<Enemy> enemiesPool = new Stack<Enemy>();

    private Spawner[] _spawners;
    public float _spawnDelay;
    public float _spawnAmount;

    private void Start()
    {
        _spawners = FindObjectsOfType<Spawner>();

        for (var i = 0; i < warmSpawnAmount; i++)
        {
            SpawnRat();
        }

        _spawnDelay = maxSpawnDelay;
        _spawnAmount = minSpawnAmount;
    }

    private void Update()
    {
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
        });
        activeEnemiesList.Add(enemy);
    }

    private Vector3 GetSpawnPosition()
    {
        return _spawners[Random.Range(0, _spawners.Length)].spawnPos.position;
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(_spawnDelay);

        for (var i = 0; i < Mathf.Floor(_spawnAmount); i++)
        {
            SpawnRat();
        }
    } 
}
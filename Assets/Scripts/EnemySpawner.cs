using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy shadowEnemyPrefab;
    [SerializeField] private Enemy lightEnemyPrefab;
    [SerializeField] private Player player;
    
    [NonSerialized] public readonly List<Enemy> EnemyPool = new List<Enemy>();

    private Spawner[] _spawners;

    private void Start()
    {
        var spawners = FindObjectsOfType<Spawner>();
        
        for (var i = 0; i < 100; i++)
        {
            SpawnRat();
        }
    }

    private void Update()
    {
        var playerPos = player.transform.position;
        foreach (var enemy in EnemyPool)
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.SetTarget(playerPos);
            }
        }
    }

    private void SpawnRat()
    {
        var pos = _spawners[Random.Range(0, _spawners.Length)].spawnPos.position;

        var enemy = Random.Range(0, 2) % 2 == 0 ? shadowEnemyPrefab : lightEnemyPrefab;
        EnemyPool.Add(Instantiate(enemy, pos, Quaternion.identity));
    }
}
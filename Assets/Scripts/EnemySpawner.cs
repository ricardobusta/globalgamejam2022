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

    private void Start()
    {
        var spawners = FindObjectsOfType<Spawner>();
        
        for (var i = 0; i < 100; i++)
        {
            var pos = spawners[Random.Range(0, spawners.Length)].spawnPos.position;

            var enemy = Random.Range(0, 2) % 2 == 0 ? shadowEnemyPrefab : lightEnemyPrefab;
            EnemyPool.Add(Instantiate(enemy, pos, Quaternion.identity));
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
}
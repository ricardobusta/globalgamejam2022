using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy blackEnemy;
    [SerializeField] private Enemy whiteEnemy;
    
    void Start()
    {
        for (var i = 0; i < 100; i++)
        {
            var pos = Random.onUnitSphere;
            pos.y = 0;
            pos = pos.normalized * 15;

            var enemy = Random.Range(0, 2) % 2 == 0 ? blackEnemy : whiteEnemy;
            Instantiate(enemy, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

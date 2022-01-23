using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool isLight;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private bool _hitable;

    public void SetTarget(Vector3 point)
    {
        navMeshAgent.SetDestination(point);
    }
    
    public void SetEnvironmentColor(bool floorIsLight)
    {
        _hitable = isLight ^ floorIsLight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hitable)
        {
            gameObject.SetActive(false);
        }
    }
}

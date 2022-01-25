using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material lightMaterial;
    [SerializeField] private Material darkMaterial;

    private bool _hittable;
    private bool _isLight;
    private Action<Enemy> _onDestroy;
    
    public void Init(bool isLight, Vector3 pos, Quaternion rot, Action<Enemy> onDestroy)
    {
        var tr = transform;
        tr.position = pos;
        tr.rotation = rot;
        _isLight = isLight;
        var mats = meshRenderer.materials;
        mats[0] = _isLight ? lightMaterial : darkMaterial;
        meshRenderer.materials = mats;
        _onDestroy = onDestroy;
    }

    public void SetTarget(Vector3 point)
    {
        navMeshAgent.SetDestination(point);
    }

    public void SetEnvironmentColor(bool floorIsLight)
    {
        _hittable = _isLight ^ floorIsLight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hittable)
        {
            gameObject.SetActive(false);
            _onDestroy?.Invoke(this);
        }
    }
}
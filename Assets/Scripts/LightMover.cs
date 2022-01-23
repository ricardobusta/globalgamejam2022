using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMover : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float centerAngle;
    [SerializeField] private float amplitude;

    private const float TwoPi = Mathf.PI * 2;
    private float _angle = 0;

    private void Update()
    {
        _angle += speed * Time.deltaTime;
        if (_angle > TwoPi)
        {
            _angle -= TwoPi;
        }

        var a = centerAngle + Mathf.Sin(_angle) * amplitude;
        var tr = transform;
        var rot = tr.rotation.eulerAngles;
        rot.y = a;
        tr.rotation = Quaternion.Euler(rot);
    }
}

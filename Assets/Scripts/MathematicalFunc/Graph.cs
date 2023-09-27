using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Graph : MonoBehaviour
{
    [SerializeField] private Transform pointCube;
    
    [SerializeField,Range(10,200)] private int resolution = 10;
    [SerializeField, Range(0, 2)] private int function = 0;
    

    private Transform[] _point;
    private void Awake()
    {
        _point = new Transform[resolution];
        
        Vector3 position = Vector3.zero;
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        for (int i = 0; i < _point.Length; i++)
        {
            Transform point = _point[i] = Instantiate(pointCube, transform, false);
            position.x = (i + 0.5f) *step - 1f;
            position.y = position.x;
            point.localPosition = position;
            point.localScale  = scale;
           
        }
    }

    private void Update()
    {
        var time = Time.time;
        for (int i = 0; i < _point.Length; i++)
        {
            Transform point = _point[i];
            Vector3 position = point.localPosition;
            if (function == 0)
            {
                position.y = FunctionalLibrary.Wave(position.x, time);
            }
            else if(function==1)
            {
                position.y = FunctionalLibrary.MultiWave(position.x, time);
            }
            else
            {
                position.y = FunctionalLibrary.Ripple(position.x, time);
            }
            point.localPosition = position;
        }
    }
}

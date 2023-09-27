using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Graph : MonoBehaviour
{
    [SerializeField] private Transform pointCube;
    
    [SerializeField,Range(10,200)] private int resolution = 10;

    [SerializeField]
    FunctionalLibrary.FunctionNames functions;
    

    private Transform[] _point;
    private void Awake()
    {
        _point = new Transform[resolution*resolution];
        
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        for (int i = 0; i < _point.Length; i++)
        {
            Transform point = _point[i] = Instantiate(pointCube, transform, false);
            
            point.localScale  = scale;
           
        }
    }

    private void Update()
    {
        FunctionalLibrary.Function fun = FunctionalLibrary.GetFunction(functions);
        
        float step = 2f / resolution;
        var time = Time.time;
        float v =  0.5f *step - 1f;
        
        for (int i = 0, x = 0, z = 0; i < _point.Length;x++, i++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = z + 0.5f * step - 1f;
            }
            
            float u = (x + 0.5f) *step - 1f;
            _point[i].localPosition = fun(u, v, time);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    private PoolGenerator poolGen;
    private void Start()
    {
        poolGen = PoolGenerator.instance;
    }

    private void FixedUpdate()
    {
        var position = transform.position;
        StartCoroutine(poolGen.Func("Sphere", position, Quaternion.identity));
    }
    
}

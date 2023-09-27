using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ApplyForce : MonoBehaviour
{
    public float upForce = 1f;
    public float sideForce = 1f;

    private void OnEnable()
    {
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(upForce/2f, upForce);
        float zForce = Random.Range(-sideForce, sideForce);

        Vector3 force = new Vector3(xForce, yForce, zForce);

        GetComponent<Rigidbody>().velocity = force;
    }

    // public void OnObjectSpawned()
    // {
    //     float xForce = Random.Range(-sideForce, sideForce);
    //     float yForce = Random.Range(upForce/2f, upForce);
    //     float zForce = Random.Range(-sideForce, sideForce);
    //
    //     Vector3 force = new Vector3(xForce, yForce, zForce);
    //
    //     GetComponent<Rigidbody>().velocity = force;
    // }
}

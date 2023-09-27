using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] _uv;
    private Color[] _colors;

    public float minTerrainHeight;
    public float maxTerrainHeight;
    public int xSize = 5;
    public int zSize = 5;
    public float strength = 0.3f;
    public float heightDistortion = 2f;
    public Gradient gradient;
    
    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        CreateMesh();
        
        
    }

    private void Update()
    {
        UpdateMesh();
    }

    private void CreateMesh()
    {
        _vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        _colors = new Color[_vertices.Length];
        
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
               var y = Mathf.PerlinNoise(x*strength, z*strength)*heightDistortion;
                _vertices[i] = new Vector3(x, y, z);
                if (y > maxTerrainHeight) y = maxTerrainHeight;
                if (y < minTerrainHeight) y = minTerrainHeight;
                //_uv[i] = new Vector2(z / (float)zSize, x / (float)xSize);
                i++;
            }
        }

        _triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tria = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                _triangles[tria + 0] = vert + 0;
                _triangles[tria + 1] = vert + xSize+1;
                _triangles[tria + 2] = vert + 1;
                _triangles[tria + 3] = vert + 1;
                _triangles[tria + 4] = vert + xSize+1;
                _triangles[tria + 5] = vert + xSize+2;
                tria+=6;
                vert++;
            }

            vert++;
        }
        
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(maxTerrainHeight,minTerrainHeight,_vertices[i].y);
                _colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    private void UpdateMesh()
    {
        //
        // for (int i = 0, z = 0; z <= zSize; z++)
        // {
        //     for (int x = 0; x <= xSize; x++)
        //     {
        //         float height = Mathf.InverseLerp(maxTerrainHeight,minTerrainHeight,_vertices[i].y);
        //         _colors[i] = gradient.Evaluate(height);
        //         i++;
        //     }
        // }
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                var y = Mathf.PerlinNoise(x*strength, z*strength)*heightDistortion*Mathf.Sin(Time.time+1f);
                _vertices[i].y = y;
                //_uv[i] = new Vector2(z / (float)zSize, x / (float)xSize);
                i++;
            }
        }
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
        _mesh.colors = _colors;
    }
    

    /*private void OnDrawGizmos()
    {
        if(_vertices == null) return;

        foreach (var point in _vertices)
        {
            Gizmos.DrawSphere(point, .1f);
        }
    }*/
}

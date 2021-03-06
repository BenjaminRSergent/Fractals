﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateTriangle : MonoBehaviour
{
    public float fps = 60.0f;
    public float wobble = 0.01f;
    public bool waitBetweenIter = false;
    public int numIterations = 4;

    private Mesh _mesh;
    private List<Vector3> _vertices = new List<Vector3>();
    private int[] _indices;

    void Start()
    {
        StartCoroutine(GenerationRoutine());
    }

    IEnumerator GenerationRoutine()
    {

        while (true)
        {
            WaitForSeconds wait = new WaitForSeconds(1 / fps);
            CreateInitialTriangle();

            for (int iteration = 0; iteration < numIterations; iteration++)
            {
                if(waitBetweenIter)
                {
                    yield return wait;
                }
                Iterate();
            }
            yield return wait;
        }
    }

    private void CreateInitialTriangle()
    {
        _mesh = new Mesh();

        _mesh.name = "Triangle";
        GetComponent<MeshFilter>().mesh = _mesh;
        _vertices = new List<Vector3>();

        // Create An equilateral triangle with side length 2 centered at (0,0,0)
        // An equilateral triangle with side length 2 has height sqrt(2^2 - 1^2) = sqrt(3)
        float height = Mathf.Sqrt(3);
        _vertices.Add(new Vector3(-1, -height/2, 0));
        _vertices.Add(new Vector3(0, height / 2, 0));
        _vertices.Add(new Vector3(1, -height / 2, 0));
        _mesh.SetVertices(_vertices);

        _indices = new int[3];
        _indices[0] = 0;
        _indices[1] = 1;
        _indices[2] = 2;
        _mesh.SetIndices(_indices, MeshTopology.Triangles, 0);
    }

    void Iterate()
    {
        var previousVerts = _vertices;
        var previousIndices = _indices;
        _vertices = new List<Vector3>();
        int prevNumTriangles = previousVerts.Count / 2;

        _indices = new int[prevNumTriangles * 9];


        int indicesIndex = 0;
        for (int triangle = 0; triangle < prevNumTriangles; triangle++)
        {
            int indexOffset = triangle * 3;

            // Use the indices to find the vertex triplets which define each triangle
            Vector3 vertA = previousVerts[previousIndices[indexOffset]];
            Vector3 vertB = previousVerts[previousIndices[indexOffset + 1]];
            Vector3 vertC = previousVerts[previousIndices[indexOffset + 2]];

            int newVertexStart = _vertices.Count;

            // Every vertex in the previous triangle is included plus a new vertex half way between
            // each previous edge
            _vertices.Add(vertA);
            _vertices.Add(vertB);
            _vertices.Add(vertC);
            _vertices.Add((vertC + vertA) / RandomUtils.RandomDiv(2.0f, wobble));
            _vertices.Add((vertA + vertB) / RandomUtils.RandomDiv(2.0f, wobble));
            _vertices.Add((vertB + vertC) / RandomUtils.RandomDiv(2.0f, wobble));

            // Make three new triangles from the six vertices. Each includes one old vertex and two new vertices.
            _indices[indicesIndex++] = newVertexStart;
            _indices[indicesIndex++] = newVertexStart + 4;
            _indices[indicesIndex++] = newVertexStart + 3;

            _indices[indicesIndex++] = newVertexStart + 4;
            _indices[indicesIndex++] = newVertexStart + 1;
            _indices[indicesIndex++] = newVertexStart + 5;

            _indices[indicesIndex++] = newVertexStart + 5;
            _indices[indicesIndex++] = newVertexStart + 2;
            _indices[indicesIndex++] = newVertexStart + 3;
        }
        _mesh.SetVertices(_vertices);
        _mesh.SetIndices(_indices, MeshTopology.Triangles, 0);
    }

    private void OnDrawGizmos()
    {
        if (_vertices == null)
        {
            return;
        }
        Gizmos.color = Color.black;
        for (int index = 0; index < _vertices.Count; index++)
        {
            // Gizmos.DrawSphere(_vertices[index], 0.1f);
            // Handles.Label(_vertices[index], "" + index);
        }
    }
}

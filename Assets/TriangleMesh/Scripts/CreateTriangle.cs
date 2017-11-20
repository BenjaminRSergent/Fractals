using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateTriangle : MonoBehaviour
{
    public int numIterations = 4;
    private Mesh _mesh;
    private List<Vector3> _vertices = new List<Vector3>();
    private int[] _indices;

    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        _mesh.name = "Triangle";
        _vertices.Add(new Vector3(-1, 0, 0));
        _vertices.Add(new Vector3(0, Mathf.Sqrt(3), 0));
        _vertices.Add(new Vector3(1, 0, 0));
        _mesh.SetVertices(_vertices);

        _indices = new int[3];
        _indices[0] = 0;
        _indices[1] = 1;
        _indices[2] = 2;
        _mesh.SetIndices(_indices, MeshTopology.Triangles, 0);

        for(int iteration = 0; iteration < numIterations; iteration++)
        {
            Iterate();
        }
        
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

            Vector3 vertA = previousVerts[previousIndices[indexOffset]];
            Vector3 vertB = previousVerts[previousIndices[indexOffset + 1]];
            Vector3 vertC = previousVerts[previousIndices[indexOffset + 2]];

            int newVertexStart = _vertices.Count;

            _vertices.Add(vertA);
            _vertices.Add(vertB);
            _vertices.Add(vertC);
            _vertices.Add((vertC + vertA) / 2.0f);
            _vertices.Add((vertA + vertB) / 2.0f);
            _vertices.Add((vertB + vertC) / 2.0f);
            

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

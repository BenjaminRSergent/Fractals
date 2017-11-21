using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatePyramid : MonoBehaviour
{
    public float fps = 60.0f;
    public float wobble = 0.01f;
    public bool waitBetweenIter = false;
    public int numIterations = 4;
    private Mesh _mesh;
    private List<Vector3> _vertices = new List<Vector3>();
    private int _currRandomIndex = 0;
    private int[] _indices;
    private float[] _randomNumbers = new float[81];

    void Start()
    {
        for (int index = 0; index < _randomNumbers.Length; index++)
        {
            _randomNumbers[index] = RandomGaussian();
        }
        StartCoroutine(GenerationRoutine());
    }

    IEnumerator GenerationRoutine()
    {

        WaitForSeconds wait = new WaitForSeconds(1 / fps);
        CreateInitialPyramid();

        yield break;
    }

    void CreateInitialPyramid()
    {
        _mesh = new Mesh();

        _mesh.name = "Quad";
        GetComponent<MeshFilter>().mesh = _mesh;
        _vertices = new List<Vector3>();
        _indices = new int[12];

        int startVertex = _vertices.Count;
        _vertices.Add(Vector3.left);
        _vertices.Add(Vector3.right);
        float forwardLen = Mathf.Sqrt(3);
        _vertices.Add(Vector3.forward * forwardLen);
        float upwardLen = Mathf.Sqrt(4 - Mathf.Pow(forwardLen, 2.0f));
        _vertices.Add(Vector3.forward * forwardLen / 2.0f + Vector3.up * upwardLen);

        int index = 0;

        _indices[index++] = 0;
        _indices[index++] = 1;
        _indices[index++] = 2;

        _indices[index++] = 3;
        _indices[index++] = 1;
        _indices[index++] = 0;

        _indices[index++] = 3;
        _indices[index++] = 2;
        _indices[index++] = 1;

        _indices[index++] = 3;
        _indices[index++] = 0;
        _indices[index++] = 2;

        _mesh.SetVertices(_vertices);
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

            Vector3 vertA = previousVerts[previousIndices[indexOffset]];
            Vector3 vertB = previousVerts[previousIndices[indexOffset + 1]];
            Vector3 vertC = previousVerts[previousIndices[indexOffset + 2]];

            int newVertexStart = _vertices.Count;

            _vertices.Add(vertA);
            _vertices.Add(vertB);
            _vertices.Add(vertC);
            _vertices.Add((vertC + vertA) / RandomDiv(2.0f));
            _vertices.Add((vertA + vertB) / RandomDiv(2.0f));
            _vertices.Add((vertB + vertC) / RandomDiv(2.0f));


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

    private float RandomDiv(float divBase)
    {
        _currRandomIndex = (_currRandomIndex + 1) % _randomNumbers.Length;
        float offset = wobble * (_randomNumbers[_currRandomIndex] - 0.5f);
        return divBase + offset;
    }

    private float RandomGaussian()
    {
        // See http://www.design.caltech.edu/erik/Misc/Gaussian.html
        float x1 = Random.Range(0, 1.0f);
        float x2 = Random.Range(0, 1.0f);

        return Mathf.Sqrt(-2 * Mathf.Log(x1)) * Mathf.Cos(2 * Mathf.PI * x2);
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

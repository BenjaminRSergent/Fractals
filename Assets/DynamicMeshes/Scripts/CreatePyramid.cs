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
    private int[] _indices;


    // Three for the base and the point
    const int VERT_PER_PYRAMID = 4;
    // Each vertex is involved in three seperate triangles. Thus, 3 indices per vertex is
    // used to make a pyramid
    const int INDICES_PER_VERT = 3;
    const int INDICES_PER_PYRAMID = VERT_PER_PYRAMID * INDICES_PER_VERT;
    // Number of new pyramids per existing pyramid during iteration
    const int PYRAMIDS_PER_PYRAMID = 4;

    void Start()
    {
        StartCoroutine(GenerationRoutine());
    }

    IEnumerator GenerationRoutine()
    {
        while (true)
        {
            WaitForSeconds wait = new WaitForSeconds(1 / fps);
            CreateInitialPyramid();

            for (int iteration = 0; iteration < numIterations; iteration++)
            {
                if (waitBetweenIter)
                {
                    yield return wait;
                }
                Iterate();
            }
            yield return wait;
        }
    }

    void CreateInitialPyramid()
    {
        _mesh = new Mesh();
        _mesh.name = "Pyramid";
        GetComponent<MeshFilter>().mesh = _mesh;

        _vertices = new List<Vector3>();
        _indices = new int[INDICES_PER_PYRAMID];

        int startVertex = _vertices.Count;

        // The height of the equilateral base
        float forwardLen = Mathf.Sqrt(3);
        // The pyramid height which makes the side triangles all equilateral with side length 2
        float upwardLen = Mathf.Sqrt(4 - Mathf.Pow(forwardLen, 2.0f));

        // The base
        _vertices.Add(Vector3.left - Vector3.forward * forwardLen / 2);
        _vertices.Add(Vector3.right - Vector3.forward * forwardLen / 2);
        _vertices.Add(Vector3.forward * forwardLen / 2);

        // The point
        _vertices.Add(Vector3.up * upwardLen);

        int index = 0;

        AddPyramidIndices(0, 1, 2, 3, ref index);

        _mesh.SetVertices(_vertices);
        _mesh.SetIndices(_indices, MeshTopology.Triangles, 0);
    }

    void Iterate()
    {
        var previousVerts = _vertices;
        var previousIndices = _indices;
        _vertices = new List<Vector3>();
        int prevNumPyramids = Mathf.Max(1, 2 * previousVerts.Count / 5);

        // Every pyramid has 4 vertices and 12 indices (4 triangles)
        int newPyramidCount = prevNumPyramids * PYRAMIDS_PER_PYRAMID;
        _indices = new int[newPyramidCount * INDICES_PER_PYRAMID];

        int indicesIndex = 0;
        for (int pyramid = 0; pyramid < prevNumPyramids; pyramid++)
        {
            int indexOffset = pyramid * INDICES_PER_PYRAMID;
            int vertOffset = _vertices.Count;

            // AddPyramidIndices is written to ensure the first three indices of each
            // pyramid are the base vertices followed by the point
            Vector3 vertA = previousVerts[previousIndices[indexOffset]];
            Vector3 vertB = previousVerts[previousIndices[indexOffset + 1]];
            Vector3 vertC = previousVerts[previousIndices[indexOffset + 2]];
            Vector3 vertD = previousVerts[previousIndices[indexOffset + 3]];

            // Every vertex in the previous pyramid is included plus a new vertex half way between
            // each previous edge
            _vertices.Add(vertA);
            _vertices.Add(vertB);
            _vertices.Add(vertC);
            _vertices.Add(vertD);
            _vertices.Add((vertA + vertB) / 2.0f + RandomUtils.RandomOffset(wobble));
            _vertices.Add((vertB + vertC) / 2.0f + RandomUtils.RandomOffset(wobble));
            _vertices.Add((vertC + vertA) / 2.0f + RandomUtils.RandomOffset(wobble));
            _vertices.Add((vertA + vertD) / 2.0f + RandomUtils.RandomOffset(wobble));
            _vertices.Add((vertB + vertD) / 2.0f + RandomUtils.RandomOffset(wobble));
            _vertices.Add((vertC + vertD) / 2.0f + RandomUtils.RandomOffset(wobble));

            AddPyramidIndices(vertOffset, vertOffset + 4, vertOffset + 6, vertOffset + 7, ref indicesIndex);
            AddPyramidIndices(vertOffset + 4, vertOffset + 1, vertOffset + 5, vertOffset + 8, ref indicesIndex);
            AddPyramidIndices(vertOffset + 6, vertOffset + 5, vertOffset + 2, vertOffset + 9, ref indicesIndex);
            AddPyramidIndices(vertOffset + 7, vertOffset + 8, vertOffset + 9, vertOffset + 3, ref indicesIndex);
        }
        _mesh.SetVertices(_vertices);
        _mesh.SetIndices(_indices, MeshTopology.Triangles, 0);
    }

    void AddPyramidIndices(int leftIndex, int rightIndex, int forwardIndex, int upIndex, ref int index)
    {
        // The fact that the first four vertices are the base (left, right, forward) followed by the point (up)
        // is used elsewhere.

        // The base
        _indices[index++] = leftIndex;
        _indices[index++] = rightIndex;
        _indices[index++] = forwardIndex;

        // The front facing side
        _indices[index++] = upIndex;
        _indices[index++] = rightIndex;
        _indices[index++] = leftIndex;

        // The right facing side
        _indices[index++] = upIndex;
        _indices[index++] = forwardIndex;
        _indices[index++] = rightIndex;

        // The left facing side
        _indices[index++] = upIndex;
        _indices[index++] = leftIndex;
        _indices[index++] = forwardIndex;
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

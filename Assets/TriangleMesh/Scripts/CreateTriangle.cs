using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTriangle : MonoBehaviour
{
    private Mesh _mesh;
    private List<Vector3> _vertices = new List<Vector3>();
    private int[] _indicies;

    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        _mesh.name = "Triangle";
        _vertices.Add(new Vector3(-1, 0, 0));
        _vertices.Add(new Vector3(0, Mathf.Sqrt(3), 0));
        _vertices.Add(new Vector3(1, 0, 0));
        _mesh.SetVertices(_vertices);

        _indicies = new int[3];
        _indicies[0] = 0;
        _indicies[1] = 1;
        _indicies[2] = 2;
        _mesh.SetIndices(_indicies, MeshTopology.Triangles, 0);

        Iterate();
    }

    void Iterate()
    {
        var previousVerts = _vertices;
        _vertices = new List<Vector3>();
        int prevNumTriangles = previousVerts.Count / 2;

        _vertices.Add(previousVerts[_indicies[0]]);
        for (int triangle = 0; triangle < prevNumTriangles; triangle++)
        {
            int indexOffset = triangle * 3;
            Vector3 vertA = previousVerts[_indicies[indexOffset]];
            Vector3 vertB = previousVerts[_indicies[indexOffset+1]];
            Vector3 vertC = previousVerts[_indicies[indexOffset + 2]];
            _vertices.Add((vertA + vertB) / 2.0f);
            _vertices.Add(vertB);
            _vertices.Add((vertB + vertC) / 2.0f);
            _vertices.Add(vertC);
            _vertices.Add((vertC + vertA) / 2.0f);
        }
        _mesh.SetVertices(_vertices);

        int numTriangles = _vertices.Count / 2;
        _indicies = new int[numTriangles*3];
        for (int triangle = 0; triangle < numTriangles; triangle++)
        {
            int indexOffset = triangle * 3;
            int vertexOffset = triangle * 2;
            _indicies[indexOffset] = (vertexOffset + 1) % _vertices.Count;
            _indicies[indexOffset + 1] = (vertexOffset + 2) % _vertices.Count;
            _indicies[indexOffset + 2] = (vertexOffset + 3) % _vertices.Count;
        }
        _mesh.SetIndices(_indicies, MeshTopology.Triangles, 0);
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
            Gizmos.DrawSphere(_vertices[index], 0.1f);
        }
    }

}

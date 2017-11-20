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
        _vertices.Add(new Vector3(0, 2, 0));
        _vertices.Add(new Vector3(1, 0, 0));
        _mesh.SetVertices(_vertices);

        _indicies = new int[3];
        _indicies[0] = 0;
        _indicies[1] = 1;
        _indicies[2] = 2;
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

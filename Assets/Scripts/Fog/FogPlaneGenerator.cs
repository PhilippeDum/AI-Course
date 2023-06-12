using System.Collections.Generic;
using UnityEngine;

public class FogPlaneGenerator : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    [SerializeField] private int resolution;
    [SerializeField] private Material material;

    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private MeshFilter meshFilter;

    private void Awake()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    private void Start()
    {
        GeneratePlane();
    }

    private void GeneratePlane()
    {
        float vertPerX = size.x / resolution;
        float vertPerZ = size.y / resolution;

        // Generate Points
        for (int z = 0; z < resolution + 1; z++)
        {
            for (int x = 0; x < resolution + 1; x++)
            {
                vertices.Add(new Vector3(x * vertPerX, 0, z * vertPerZ));
            }
        }

        // Generate Triangles
        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = (z * resolution) + z + x;

                triangles.Add(i);
                triangles.Add(i + resolution + 1);
                triangles.Add(i + resolution + 2);

                triangles.Add(i);
                triangles.Add(i + resolution + 2);
                triangles.Add(i + 1);
            }
        }

        SetupPlane();
    }

    private void SetupPlane()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        GetComponent<MeshRenderer>().sharedMaterial = material;

        transform.position = new Vector3(-size.x / 2, transform.position.y, -size.y / 2);

        gameObject.AddComponent<BoxCollider>();
    }
}
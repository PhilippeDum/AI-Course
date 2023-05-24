using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fog : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;
    private LayerMask fogLayer;
    private GameObject fog;

    public static event Action OnCompleteInitialize;

    public static Fog instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        fogLayer = LayerMask.GetMask("Fog");

        fog = GameObject.Find("Fog");

        Initialize();
    }

    #region Methods

    private void Initialize()
    {
        if (fog == null) return;

        mesh = fog.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            
            colors[i] = Color.black * Random.Range(0.995f, 1.000f);
        }

        mesh.colors = colors;

        OnCompleteInitialize?.Invoke();
    }

    public void UnhideUnit(Transform unit, int radius)
    {
        if (fog == null) return;

        Mesh unitMesh = unit.GetComponent<MeshFilter>().mesh;
        Vector3[] unitVertices = unitMesh.vertices;

        foreach (var vertice in unitVertices)
        {
            Vector3 verticePos = unit.TransformPoint(vertice);
            Ray ray = new Ray(transform.position, verticePos - transform.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1500, fogLayer, QueryTriggerInteraction.Collide))
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 vPos = fog.transform.TransformPoint(vertices[i]);
                    float distance = Vector3.SqrMagnitude(vPos - hit.point);

                    if (distance < radius)
                    {
                        float alpha = Mathf.Min(colors[i].a, distance / radius);
                        colors[i].a = alpha;
                    }
                }

                mesh.colors = colors;
            }
        }
    }

    #endregion
}
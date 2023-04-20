using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BridgeReparation : MonoBehaviour
{
    [Header("Bridges")]
    [SerializeField] private GameObject brokenBridge;
    [SerializeField] private GameObject repairedBridge;
    [SerializeField] private float timeOfReparation = 5f;
    [SerializeField] private NavMeshSurface surface;

    private void Start()
    {
        brokenBridge.SetActive(true);
        repairedBridge.SetActive(false);

        /*surface.BuildNavMesh();
        surface.UpdateNavMesh(surface.navMeshData);*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Repair();
        }
    }

    public void Repair()
    {
        StartCoroutine(Reparation());
    }

    private IEnumerator Reparation()
    {
        yield return new WaitForSeconds(timeOfReparation);

        brokenBridge.SetActive(false);
        repairedBridge.SetActive(true);

        surface.BuildNavMesh();
        surface.UpdateNavMesh(surface.navMeshData);
    }
}
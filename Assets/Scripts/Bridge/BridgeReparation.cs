using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class BridgeReparation : MonoBehaviour
{
    [Header("Bridges")]
    [SerializeField] private GameObject brokenBridge;
    [SerializeField] private GameObject repairedBridge;
    [SerializeField] private float timeOfReparation = 5f;
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private Slider reparationSlider;
    [SerializeField][Range(0, 1)] private float lerp = 0.5f;

    private void Start()
    {
        reparationSlider.transform.parent.gameObject.SetActive(false);

        brokenBridge.SetActive(true);
        repairedBridge.SetActive(false);
    }

    private void Update()
    {
        if (reparationSlider == null) return;

        reparationSlider.transform.parent.LookAt(Camera.main.transform.position);

        reparationSlider.value = Mathf.Lerp(reparationSlider.value, 0f, lerp * Time.deltaTime);
    }

    public void Repair()
    {
        StartCoroutine(Reparation());
    }

    private IEnumerator Reparation()
    {
        reparationSlider.maxValue = timeOfReparation;
        reparationSlider.value = timeOfReparation;

        reparationSlider.transform.parent.gameObject.SetActive(true);

        yield return new WaitForSeconds(timeOfReparation);

        brokenBridge.SetActive(false);
        repairedBridge.SetActive(true);

        surface.BuildNavMesh();
        surface.UpdateNavMesh(surface.navMeshData);

        GameManager.instance.CurrentBridgeRepaired = true;
    }
}
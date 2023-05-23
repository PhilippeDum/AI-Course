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

    private float timeRemaining = 0f;
    private bool canRepair = false;
    private bool inReparation = false;

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

        //reparationSlider.value = Mathf.Lerp(reparationSlider.value, 0f, lerp * Time.deltaTime);

        if (canRepair)
        {
            canRepair = false;

            timeRemaining = timeOfReparation;

            reparationSlider.maxValue = timeRemaining;

            inReparation = true;
        }

        if (inReparation)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                reparationSlider.value = timeRemaining;
                
                reparationSlider.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                brokenBridge.SetActive(false);
                repairedBridge.SetActive(true);

                surface.BuildNavMesh();
                surface.UpdateNavMesh(surface.navMeshData);

                GameManager.instance.CurrentBridgeRepaired = true;

                inReparation = false;
            }
        }
    }


    public void Repair()
    {
        //StartCoroutine(Reparation());

        canRepair = true;
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
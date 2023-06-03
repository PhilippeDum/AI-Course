using System.Collections.Generic;
using UnityEngine;

public class BlueprintManager : MonoBehaviour
{
    [SerializeField] private Material invalidMaterial;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private int defoggerRadius = 1000;
    [SerializeField] private List<BuildingDatas> datas = new List<BuildingDatas>();

    private GameObject blueprintGO;
    private BlueprintController blueprint;
    private Material defaultMaterial;
    private int layerMaskTerrain = 1 << 6;
    private Fog fog;
    private int dataIndex;

    private void Start()
    {
        fog = Fog.instance;
    }

    private void Update()
    {
        if (blueprintGO)
        {
            SetMaterial();

            CursorControls();
        }
    }

    public void FindBuilding(string name)
    {
        if (name == string.Empty) return;

        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].Name == name)
            {
                dataIndex = i;

                FindObjectOfType<CameraZoom>().CanZoom = false;

                InstantiateBuilding(datas[i].Prefab);
            }
        }
    }

    private void InstantiateBuilding(GameObject building)
    {
        blueprintGO = Instantiate(building, Vector3.zero, Quaternion.identity);
        blueprint = blueprintGO.GetComponent<BlueprintController>();
        defaultMaterial = blueprintGO.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;
    }

    private void CursorControls()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 150, layerMaskTerrain))
        {
            //blueprintGO.transform.position = new Vector3(hit.point.x, blueprintGO.transform.position.y, hit.point.z);
            blueprintGO.transform.position = hit.point;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            blueprintGO.transform.Rotate(Vector3.up * rotationSpeed, Space.Self);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            blueprintGO.transform.Rotate(-Vector3.up * rotationSpeed, Space.Self);
        }

        if (Input.GetMouseButton(0))
        {
            if (blueprint.CheckValidPlacement()) PlaceBlueprint();
        }
    }

    private void PlaceBlueprint()
    {
        fog.UnhideUnit(blueprintGO.transform, defoggerRadius);

        blueprintGO.GetComponent<Building>().Datas = datas[dataIndex];

        blueprintGO.GetComponent<Building>().enabled = true;

        blueprintGO = null;

        Destroy(blueprint);

        FindObjectOfType<CameraZoom>().CanZoom = true;
    }

    private void SetMaterial()
    {
        if (blueprint.CheckValidPlacement())
        {
            Renderer[] chidlRenderers = blueprintGO.transform.GetChild(0).GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in chidlRenderers)
            {
                renderer.sharedMaterial = defaultMaterial;
            }

            //transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = defaultMaterial;
        }
        else
        {
            Renderer[] chidlRenderers = blueprintGO.transform.GetChild(0).GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in chidlRenderers)
            {
                renderer.sharedMaterial = invalidMaterial;
            }

            //transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = invalidMaterial;
        }
    }
}
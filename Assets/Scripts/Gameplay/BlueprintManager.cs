using System;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintManager : MonoBehaviour
{
    public static BlueprintManager instance;

    [Header("References")]
    [SerializeField] private Transform playerBuildings;
    [SerializeField] private GameObject playerInteractionArea;
    [SerializeField] private Material invalidMaterial;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private int defoggerRadius = 1000;
    [SerializeField] private List<BuildingDatas> datas = new List<BuildingDatas>();
    [SerializeField] private bool placeBuilding = false;

    private GameObject blueprintGO;
    private BlueprintController blueprint;
    private Material defaultMaterial;
    private int layerMaskTerrain = 1 << 6;
    private Fog fog;
    private int dataIndex;

    public event Action OnBuildingPlaced;

    public GameObject PlayerInteractionArea
    {
        get { return playerInteractionArea; }
        set { playerInteractionArea = value; }
    }

    public bool PlaceBuilding
    {
        get { return placeBuilding; }
        set { placeBuilding = value; }
    }

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
        if (blueprintGO != null)
        {
            Destroy(blueprintGO);

            blueprintGO = null;
            blueprint = null;
        }

        if (name == string.Empty) return;

        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].Name == name)
            {
                if (datas[i].CountBuildings < datas[i].MaxCountBuildings)
                {
                    dataIndex = i;

                    placeBuilding = true;

                    InstantiateBuilding(datas[i].Prefab);
                }
                else
                    Debug.Log($"{datas[i].CountBuildings}/{datas[i].MaxCountBuildings} ==> limit reached for {datas[i].Name}");
            }
        }
    }

    private void InstantiateBuilding(GameObject building)
    {
        blueprintGO = Instantiate(building, Vector3.zero, Quaternion.identity);
        blueprintGO.transform.SetParent(playerBuildings);
        blueprint = blueprintGO.GetComponent<BlueprintController>();

        if (blueprintGO.transform.GetChild(0).GetComponent<Renderer>())
            defaultMaterial = blueprintGO.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;
        else
            defaultMaterial = blueprintGO.transform.GetChild(0).GetComponentInChildren<Renderer>().sharedMaterial;
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

        if (Input.GetMouseButtonDown(1) && blueprintGO != null)
        {
            Destroy(blueprintGO);

            blueprintGO = null;
            blueprint = null;
        }
    }

    private void PlaceBlueprint()
    {
        if (blueprintGO == null) return;

        //fog.UnhideUnit(blueprintGO.transform, defoggerRadius);

        Transform meshFog = null;

        if (blueprintGO.GetComponent<UnitManager>())
            meshFog = blueprintGO.GetComponent<UnitManager>().DefoggerMesh.transform;
        else if (blueprintGO.GetComponent<Building>())
            meshFog = blueprintGO.GetComponent<Building>().DefoggerMesh.transform;

        if (meshFog != null)
            fog.UnhideUnit(meshFog, defoggerRadius);

        if (blueprintGO.GetComponent<Building>())
        {
            blueprintGO.GetComponent<Building>().Datas = datas[dataIndex];

            blueprintGO.GetComponent<Building>().enabled = true;
        }

        blueprintGO = null;

        Destroy(blueprint);

        placeBuilding = false;

        OnBuildingPlaced?.Invoke();
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
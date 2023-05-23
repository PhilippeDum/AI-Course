using UnityEngine;

public class BlueprintManager : MonoBehaviour
{
    [SerializeField] private GameObject building;
    [SerializeField] private Material invalidMaterial;
    [SerializeField] private float rotationSpeed = 10f;

    private GameObject blueprintGO;
    private BlueprintController blueprint;
    private Material defaultMaterial;
    private int layerMaskTerrain = 1 << 9;

    private void Update()
    {
        if (blueprintGO)
        {
            SetMaterial();

            CursorControls();
        }
    }

    public void InstantiateBuilding()
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
        blueprintGO = null;

        Destroy(blueprint);
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
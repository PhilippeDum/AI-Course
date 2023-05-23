using UnityEngine;

public class BlueprintController : MonoBehaviour
{
    [SerializeField] private Material invalidMaterial;

    private int layerMaskTerrain = 1 << 9;
    private int nbCollision = 0;
    private bool hasValidPlacement = true;
    private Material defaultMaterial;

    private void Start()
    {
        defaultMaterial = transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;
    }

    private void Update()
    {
        FollowCursor();

        CheckValidPlacement();

        SetMaterial();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            nbCollision++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            nbCollision--;
        }
    }

    #region Custom Methods

    private void FollowCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 150, layerMaskTerrain))
        {
            transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            //transform.position = hit.point;
        }
    }

    private void CheckValidPlacement()
    {
        if (nbCollision > 0)
        {
            hasValidPlacement = false;
        }
        else
        {
            hasValidPlacement = true;
        }
    }

    private void SetMaterial()
    {
        if (hasValidPlacement)
        {
            transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = defaultMaterial;
        }
        else
        {
            transform.GetChild(0).GetComponent<Renderer>().sharedMaterial = invalidMaterial;
        }
    }

    #endregion
}
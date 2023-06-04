using UnityEngine;

public class BlueprintController : MonoBehaviour
{
    [SerializeField] private float marge = 0.1f;

    private int nbCollision = 0;
    private BoxCollider boxCollider;
    private int layerMaskTerrain = 1 << 6;
    private float previousDistance = 0;

    private void OnEnable()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        CheckValidPlacement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Building") 
            || other.gameObject.layer == LayerMask.NameToLayer("Unit")
            || other.gameObject.layer == LayerMask.NameToLayer("Resource"))
        {
            nbCollision++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Building") 
            || other.gameObject.layer == LayerMask.NameToLayer("Unit")
            || other.gameObject.layer == LayerMask.NameToLayer("Resource"))
        {
            nbCollision--;
        }
    }

    #region Custom Methods

    private bool CheckInteractionArea()
    {
        GameObject playerInteractionArea = BlueprintManager.instance.PlayerInteractionArea;

        if (playerInteractionArea == null) return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool inInteractionArea = false;

        if (Physics.Raycast(ray, out hit, 150, layerMaskTerrain))
        {
            if (hit.transform == playerInteractionArea.transform) inInteractionArea = true;
        }

        return inInteractionArea;
    }

    public bool CheckValidPlacement()
    {
        if (!CheckInteractionArea()) return false;

        if (nbCollision > 0)
        {
            return false;
        }
        else
        {
            Vector3 position = transform.position;
            Vector3 center = boxCollider.center;
            Vector3 size = boxCollider.size / 2;

            float pointY = center.y - size.y;

            Vector3[] corners = new Vector3[]
            {
            new Vector3(center.x - size.x, pointY, center.z - size.z),
            new Vector3(center.x - size.x, pointY, center.z + size.z),
            new Vector3(center.x + size.x, pointY, center.z - size.z),
            new Vector3(center.x + size.x, pointY, center.z + size.z),
            };

            RaycastHit hit;

            foreach (Vector3 corner in corners)
            {
                if (Physics.Raycast(position + corner, -Vector3.up, out hit, 5, layerMaskTerrain))
                {
                    if (previousDistance != 0)
                    {
                        if (previousDistance > hit.distance + marge || previousDistance < hit.distance - marge)
                        {
                            return false;
                        }
                    }

                    previousDistance = hit.distance;
                }
            }

            return true;
        }
    }

    #endregion
}
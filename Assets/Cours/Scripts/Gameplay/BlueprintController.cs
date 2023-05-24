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

    public bool CheckValidPlacement()
    {
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
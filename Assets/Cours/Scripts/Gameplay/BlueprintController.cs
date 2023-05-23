using UnityEngine;

public class BlueprintController : MonoBehaviour
{
    private int nbCollision = 0;

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
            return true;
        }
    }

    #endregion
}
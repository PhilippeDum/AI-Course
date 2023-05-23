using UnityEngine;

public class BlueprintManager : MonoBehaviour
{
    [SerializeField] private GameObject generator;

    public void InstantiateBuilding()
    {
        Instantiate(generator, Vector3.zero, Quaternion.identity);
    }
}
using UnityEngine;

public class Element : MonoBehaviour
{
    [SerializeField] private string elementName;
    [SerializeField] private string elementDescription;
    [SerializeField] private string elementButtonName;

    public string ElementName
    {
        get { return elementName; }
        set { elementName = value; }
    }

    public string ElementDescription
    {
        get { return elementDescription; }
        set { elementDescription = value; }
    }

    public string ElementButtonName
    {
        get { return elementButtonName; }
        set { elementButtonName = value; }
    }
}
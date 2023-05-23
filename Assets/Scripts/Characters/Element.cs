using UnityEngine;

public class Element : MonoBehaviour
{
    [SerializeField] private string elementName;
    [SerializeField] private string elementDescription;
    [SerializeField] private string elementFirstButtonName;
    [SerializeField] private string elementSecondButtonName;
    [SerializeField] private GameObject elementSelection;

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

    public string ElementFirstButtonName
    {
        get { return elementFirstButtonName; }
        set { elementFirstButtonName = value; }
    }

    public string ElementSecondButtonName
    {
        get { return elementSecondButtonName; }
        set { elementSecondButtonName = value; }
    }

    public GameObject ElementSelection
    {
        get { return elementSelection; }
        set { elementSelection = value; }
    }
}
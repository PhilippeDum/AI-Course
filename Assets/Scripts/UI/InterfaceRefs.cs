using UnityEngine;
using UnityEngine.UI;

public class InterfaceRefs : MonoBehaviour
{
    [SerializeField] private Text infosTitle;
    [SerializeField] private Text infosDescription;
    [SerializeField] private GameObject buildingsButtons;
    [SerializeField] private Text optionsTitle;
    [SerializeField] private Text optionsDescription;

    #region Getters / Setters

    public Text InfosTitle
    {
        get { return infosTitle; }
        set { infosTitle = value; }
    }

    public Text InfosDescription
    {
        get { return infosDescription; }
        set { infosDescription = value; }
    }

    public GameObject BuildingsButtons
    {
        get { return buildingsButtons; }
        set { buildingsButtons = value; }
    }

    public Text OptionsTitle
    {
        get { return optionsTitle; }
        set { optionsTitle = value; }
    }

    public Text OptionsDescription
    {
        get { return optionsDescription; }
        set { optionsDescription = value; }
    }

    #endregion
}
using UnityEngine;
using UnityEngine.UI;

public class InterfaceRefs : MonoBehaviour
{
    [Header("Resources Refs")]
    [SerializeField] private Text woodCount;
    [SerializeField] private Text ironCount;
    [SerializeField] private Text goldCount;

    [Header("Units/Buildings Refs")]
    [SerializeField] private Text infosTitle;
    [SerializeField] private Text infosDescription;
    [SerializeField] private GameObject buildingsButtons;
    [SerializeField] private Text optionsTitle;
    [SerializeField] private Text optionsDescription;

    #region Getters / Setters

    public Text WoodCount
    {
        get { return woodCount; }
        set { woodCount = value; }
    }

    public Text IronCount
    {
        get { return ironCount; }
        set { ironCount = value; }
    }

    public Text GoldCount
    {
        get { return goldCount; }
        set { goldCount = value; }
    }

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
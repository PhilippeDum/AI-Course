using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform brokenBridgeTarget;

    [Header("Camera - Broken Bridge Datas")]
    [SerializeField] private float smoothing = 5f;
    [SerializeField] private float timeShowingBrokenBridge = 3f;
    [SerializeField] private bool startShowing = false;
    [SerializeField] private bool stopShowing = false;

    private Vector3 lastPosition;

    public bool StartShowing
    {
        get { return startShowing; }
        set { startShowing = value; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !startShowing)
        {
            startShowing = true;
        }

        if (startShowing) ActiveCameraShow();

        if (stopShowing) StartCoroutine(ReturnLastPosition());
    }

    #region Handle Camera to Broken Bridge

    private void ActiveCameraShow()
    {
        cameraRoot.GetComponent<CameraMotion>().CanMove = false;

        StartCoroutine(ShowBrokenBridge());
    }

    private IEnumerator ShowBrokenBridge()
    {
        lastPosition = cameraRoot.position;

        cameraRoot.position = Vector3.Lerp(cameraRoot.position, brokenBridgeTarget.position, Time.deltaTime * smoothing);

        yield return new WaitForSeconds(timeShowingBrokenBridge);

        stopShowing = true;
        startShowing = false;
    }

    private IEnumerator ReturnLastPosition()
    {
        cameraRoot.position = Vector3.Lerp(cameraRoot.position, lastPosition, Time.deltaTime * smoothing);

        yield return new WaitForSeconds(timeShowingBrokenBridge);

        cameraRoot.GetComponent<CameraMotion>().CanMove = true;

        stopShowing = false;
    }

    #endregion
}
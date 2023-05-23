using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float timer;
    [SerializeField] private bool timeStarted;
    [SerializeField] private Text timerText;

    private void Start()
    {
        timeStarted = true;
    }

    private void Update()
    {
        if (timeStarted) timer += Time.deltaTime;

        HandleTimer();
    }

    private void HandleTimer()
    {
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");

        timerText.text = $"{minutes} : {seconds}";
    }
}
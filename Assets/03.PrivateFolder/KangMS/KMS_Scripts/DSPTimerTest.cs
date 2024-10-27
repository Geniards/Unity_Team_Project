using UnityEngine;
using UnityEngine.UI;

public class DSPTimerTest : MonoBehaviour
{
    public Text deltaTimeText;
    public Text dspTimeText;

    private double dspStartTime;
    private float accumulatedDeltaTime;
    private bool isRunning = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !isRunning)
        {
            dspStartTime = AudioSettings.dspTime;
            accumulatedDeltaTime = 0f;
            isRunning = true;
        }

        if (isRunning)
        {
            accumulatedDeltaTime += Time.deltaTime;
            double dspElapsedTime = AudioSettings.dspTime - dspStartTime;

            deltaTimeText.text = $"누적 Delta Time: {accumulatedDeltaTime:F3} 초";
            dspTimeText.text = $"누적 DSP Time: {dspElapsedTime:F3} 초";
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            isRunning = false;
        }
    }
}

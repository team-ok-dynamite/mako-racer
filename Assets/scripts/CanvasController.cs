using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    public MakoController mc;

    public UnityEngine.UI.Text stopwatchText;
    public UnityEngine.UI.Text messagesText;

    private System.Diagnostics.Stopwatch raceClock;
    private bool started;

	void Start()
    {
        mc.raceHasBegun = false;
        raceClock = new System.Diagnostics.Stopwatch();
        StartCoroutine(runStartCountdown());
	}

	void Update()
    {
        displayElapsedTime();
        if (mc.finishedRace)
        {
            raceClock.Stop();
            messagesText.text = "Finished!\n" + formatRaceTime();
        }
	}

    private IEnumerator runStartCountdown()
    {
        int startCountdown = 5;
        while (startCountdown > 0)
        {
            displayStartWarning(startCountdown);
            yield return new WaitForSeconds(1);
            startCountdown -= 1;
        }
        messagesText.text = "GO!";

        raceClock.Start();
        mc.raceHasBegun = true;

        yield return new WaitForSeconds(1);
        messagesText.text = "";
    }

    private void displayStartWarning(int secToStart)
    {
        messagesText.text = "Mission will begin in..." + secToStart;
    }

    private void displayElapsedTime()
    {
        stopwatchText.text = formatRaceTime();
    }

    private string formatRaceTime()
    {
        long ms = raceClock.ElapsedMilliseconds;
        System.TimeSpan ts = System.TimeSpan.FromMilliseconds(ms);
        return string.Format("{0:D2}:{1:D2}:{2:D3}", ts.Minutes, ts.Seconds, ts.Milliseconds);
    }
}

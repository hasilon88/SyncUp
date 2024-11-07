using System;
using UnityEngine;

public class TempoSetter : MonoBehaviour
{

    private float realTimeSnapshot = 0f;
    private bool inFrameWindow = false;
    private int frameCount = 0;
    private float secondsPerBeat = 0f;
    public float Tempo = 100f;
    public int FrameWindow = 15;
    public bool CurrentlyOnBeat = false;
    public event EventHandler OnBeat; //for abilities (perfect hit)
    public event EventHandler OnBeatLeave;
    public event EventHandler OnTempoChange; //to add effects on tempoChange

    private void Start()
    {
        SetSecondsPerBeat();
        OnTempoChange += SetSecondsPerBeatCallback;
    }

    private void SetSecondsPerBeat()
    {
        secondsPerBeat = 60f / Tempo;
    }


    private void SetSecondsPerBeatCallback(object sender, EventArgs e)
    {
        SetSecondsPerBeat();
    }

    public void SetTempo(float nTempo)
    {
        Tempo = nTempo;
        OnTempoChange?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateTempoState() //use in audioManagerCapture?
    {
        if (inFrameWindow)
        {
            OnBeat?.Invoke(this, EventArgs.Empty);
            frameCount++;
        }

        if (frameCount >= FrameWindow && inFrameWindow)
        {
            frameCount = 0;
            inFrameWindow = false;
            CurrentlyOnBeat = false;
            realTimeSnapshot = GlobalStates.Instance.RealTime;
            OnBeatLeave?.Invoke(this, EventArgs.Empty);
        }
        else if (GlobalStates.Instance.RealTime - realTimeSnapshot > secondsPerBeat && !inFrameWindow)
        {
            CurrentlyOnBeat = true;
            inFrameWindow = true;
        }
    }

    private void Update()
    {
        UpdateTempoState();
    }
}

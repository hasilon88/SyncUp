using System;
using System.Threading.Tasks;
using UnityEngine;

public class TempoSetter : MonoBehaviour
{

    private bool inFrameWindow = false;
    private int frameCount = 0;
    private TimingController timingController;
    private SpotifyController spotifyController;
    public float Tempo = 100f;
    public int FrameWindow = 15;
    public bool CurrentlyOnBeat = false;
    public event EventHandler OnBeat; //for abilities (perfect hit)
    public event EventHandler OnBeatLeave;
    public event EventHandler OnTempoChange; //to add effects on tempoChange

    private void Start()
    {
        spotifyController = SpotifyController.Instance;
        if (spotifyController != null)
        {
            spotifyController.OnNext += HandleSongChange;
            spotifyController.OnPrevious += HandleSongChange;
        }
        timingController = GetComponent<TimingController>();
        OnTempoChange += (object sender, EventArgs e) => timingController.Target = 60f / Tempo;
    }

    /// <summary>
    /// TASK = *
    /// VOID = EVENTS
    /// https://stackoverflow.com/questions/12144077/async-await-when-to-return-a-task-vs-void
    /// </summary>
    public async void HandleSongChange(object sender, EventArgs e)
    {
        Song song = await spotifyController.GetCurrentlyPlayingSong();
        SetTempo(song.Tempo);
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
            OnBeatLeave?.Invoke(this, EventArgs.Empty);
        }
        else if (timingController.IsOnTime)
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

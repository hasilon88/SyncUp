
using System;
using UnityEngine;

public class PlayerCameraRewinder : MonoBehaviour, IRewind
{

    private RewindArray<Vector3> lastEulerAngles; //(y, z, x)
    private int rewindOffset = 0;
    public RewindAbility RewindAbility;

    private void Start()
    {
        //NEED TO AVOID REPETITION
        RewindAbility.OnRewindIteration += (object sender, EventArgs e) => rewindOffset++;
        RewindAbility.OnRewindStop += (object sender, EventArgs e) => rewindOffset = 0;
        lastEulerAngles = new RewindArray<Vector3>(GlobalStates.Instance.RewindArrayLength);
    }

    public void Rewind()
    {
        transform.localEulerAngles = lastEulerAngles.GetLast(rewindOffset);
    }

    public void UpdateRewindElements()
    {
        lastEulerAngles.Add(transform.localEulerAngles);
    }

}

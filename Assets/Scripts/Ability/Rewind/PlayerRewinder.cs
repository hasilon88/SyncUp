using System;
using UnityEngine;

public class PlayerRewinder : MonoBehaviour, IRewind
{

    private RewindArray<Vector3> lastEulerAngles; //(y, z, x)
    private RewindArray<Vector3> lastPositions;
    private int rewindOffset = 0;
    public RewindAbility RewindAbility;

    private void Start()
    {
        //NEED TO AVOID REPETITION
        RewindAbility.OnRewindIteration += (object sender, EventArgs e) => rewindOffset++;
        RewindAbility.OnRewindStop += (object sender, EventArgs e) =>
        {
            rewindOffset = 0;
            lastEulerAngles = lastEulerAngles.Reset();
            lastPositions = lastPositions.Reset();
        };
        lastEulerAngles = new RewindArray<Vector3>();
        lastPositions = new RewindArray<Vector3>();
    }

    public RewindResponse Rewind()
    {
        RewindResponse cameraRes = lastEulerAngles.GetLast(rewindOffset);
        RewindResponse positionRes = lastPositions.GetLast(rewindOffset);
        positionRes.RewindingObject = this.gameObject;
        transform.localEulerAngles = (Vector3)cameraRes.Element;
        transform.position = (Vector3)positionRes.Element;
        return positionRes;
    }

    public void UpdateRewindElements()
    {
        lastEulerAngles.Add(transform.localEulerAngles);
        lastPositions.Add(transform.position);
    }

}
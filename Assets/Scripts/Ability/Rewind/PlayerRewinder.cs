using System;
using UnityEngine;

public class PlayerRewinder : IRewind
{

    private RewindArray<Vector3> lastEulerAngles; //(y, z, x)
    private RewindArray<Vector3> lastPositions;

    public override void ResetRewindProperties()
    {
        rewindOffset = 0;
        Debug.Log(" TEST : " + rewindAbility.GetRewindDurationInFrames());
        lastEulerAngles = new RewindArray<Vector3>(rewindAbility.GetRewindDurationInFrames());
        lastPositions = new RewindArray<Vector3>(rewindAbility.GetRewindDurationInFrames());
    }

    public override RewindResponse Rewind()
    {
        RewindResponse cameraRes = lastEulerAngles.GetLast(rewindOffset);
        RewindResponse positionRes = lastPositions.GetLast(rewindOffset);
        positionRes.RewindingObject = gameObject;
        transform.localEulerAngles = (Vector3)cameraRes.Element;
        transform.position = (Vector3)positionRes.Element;
        return positionRes;
    }

    public override void UpdateRewindElements()
    {
        lastEulerAngles.Add(transform.localEulerAngles);
        lastPositions.Add(transform.position);
    }

}
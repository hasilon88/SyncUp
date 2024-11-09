using System;
using UnityEngine;

/// <summary>
/// LIMITED REWIND SCRIPT (ONLY REWINDS WORLD POSITION)
/// </summary>
public class SimpleVectorRewinder : IRewind
{

    private RewindArray<Vector3> lastPositions;

    public override void ResetRewindProperties()
    {
        lastPositions = new RewindArray<Vector3>(rewindAbility.GetRewindDurationInFrames());
    }

    public override RewindResponse Rewind()
    {
        RewindResponse res = lastPositions.GetLast(rewindOffset);
        res.RewindingObject = this.gameObject;
        transform.position = (Vector3)res.Element;
        return res; //or null
    }

    public override void UpdateRewindElements()
    {
        lastPositions.Add(transform.position);
    }
}

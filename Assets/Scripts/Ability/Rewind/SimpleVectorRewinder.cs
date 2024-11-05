using System;
using UnityEngine;

/// <summary>
/// LIMITED REWIND SCRIPT (ONLY REWINDS WORLD POSITION)
/// </summary>
public class SimpleVectorRewinder : MonoBehaviour, IRewind
{

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
            lastPositions = lastPositions.Reset();
        };
        lastPositions = new RewindArray<Vector3>();
    }

    public RewindResponse Rewind()
    {
        RewindResponse res = lastPositions.GetLast(rewindOffset);
        res.RewindingObject = this.gameObject;
        transform.position = (Vector3)res.Element;
        return res; //or null
    }

    public void UpdateRewindElements()
    {
        lastPositions.Add(transform.position);
    }
}

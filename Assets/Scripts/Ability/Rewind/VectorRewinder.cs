using System;
using UnityEngine;

/// <summary>
/// LIMITED REWIND SCRIPT (ONLY REWINDS WORLD POSITION)
/// </summary>
public class VectorRewinder : MonoBehaviour, IRewind
{

    private RewindArray<Vector3> lastPositions;
    private int rewindOffset = 0;
    public RewindAbility RewindAbility;

    private void Start()
    {
        RewindAbility.OnRewindIteration += (object sender, EventArgs e) => rewindOffset++;
        RewindAbility.OnRewindStop += (object sender, EventArgs e) => rewindOffset = 0;
        lastPositions = new RewindArray<Vector3>(20);
    }

    public void Rewind()
    {
        transform.position = lastPositions.GetLast(rewindOffset);
    }

    public void UpdateRewindElements()
    {
        lastPositions.Add(transform.position);
    }
}

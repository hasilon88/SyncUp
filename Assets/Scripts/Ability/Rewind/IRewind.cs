using System;
using UnityEngine;

/// <summary>
/// ...
/// </summary>
public abstract class IRewind : MonoBehaviour
{

    protected int rewindOffset = 0;
    protected RewindAbility rewindAbility;

    private void Start()
    {
        rewindAbility = GameObject.FindGameObjectWithTag("RewindAbility").GetComponent<RewindAbility>();
        rewindAbility.OnRewindIteration += (object sender, EventArgs e) => rewindOffset++;
        rewindAbility.OnRewindStop += (object sender, EventArgs e) => rewindOffset = 0;
        rewindAbility.OnRewindStop += (object sender, EventArgs e) => ResetRewindProperties();
        ResetRewindProperties();
    }

    public abstract void UpdateRewindElements();
    public abstract RewindResponse Rewind();
    public abstract void ResetRewindProperties();


}

/// <summary>
/// FOR PLAYER REWINDER (TO STOP REWIND EARLIER)
/// </summary>
public class RewindResponse
{
    public object Element { get; set; }
    public Boolean HasToStop { get; set; }
    public GameObject RewindingObject { get; set; }

    public RewindResponse(object element, Boolean hasToStop, GameObject gameObject) 
    {
        this.Element = element;
        this.HasToStop = hasToStop;
        this.RewindingObject = gameObject;
    }
}
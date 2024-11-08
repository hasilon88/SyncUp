using System;
using UnityEngine;

/// <summary>
/// Rewinder will search for everyting implementing this interface
/// </summary>
public interface IRewind
{

    public abstract void UpdateRewindElements();
    public abstract RewindResponse Rewind();
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
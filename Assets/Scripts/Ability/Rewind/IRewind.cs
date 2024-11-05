
//IRewind<T>
//abstract T[] lastElements; 
using System;
using UnityEngine;

/// <summary>
/// Rewinder will search for everyting implementing this interface
/// </summary>
public interface IRewind
{

    public abstract void UpdateRewindElements();
    public abstract void Rewind();
}

/// <summary>
/// FOR PLAYER REWINDER (TO STOP REWIND EARLIER)
/// </summary>
public class RewindResponse
{
    public object Element;
    public Boolean HasToStop;
    public GameObject RewindingObject;

}
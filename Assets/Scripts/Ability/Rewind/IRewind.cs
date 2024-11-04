using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IRewind<T>
//abstract T[] lastElements; 
public interface IRewind
{

    public abstract void UpdateRewindElements();
    public abstract void Rewind();
}

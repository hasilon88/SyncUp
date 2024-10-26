using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindAbility : Ability
{

    private IRewind[] RewindableObjects;
    public FPSManager FPSManager;


    public void Start()
    {
        
    }

    private void UpdateRewindableObjects()
    {

    }

    public IEnumerator Rewind(int seconds)
    {
        while (true) //while (secodns in in-game time)
        {
            yield return null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(this.triggerKey)) {
        
        }
    }
}

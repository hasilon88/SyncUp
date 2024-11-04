using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class RewindAbility : Ability
{
    [SerializeField]
    private IRewind[] rewindableObjects;
    public int RewindDurationInSeconds = 3;
    private int realtime = 0;
    public int RewindElementsAddTimeThresold = 15; //new name?
    private int lastRewindElementsAddRealtime = 0;

    private event EventHandler onRewindStart;
    private event EventHandler onRewindIteration;
    private event EventHandler onRewindStop;

    private event EventHandler onRewindElementsAddStart;
    private event EventHandler onRewindElementsAddStop;

    public void Start()
    {
        onRewindStart += (object sender, EventArgs e) => Debug.Log("Rewind started");
        onRewindIteration += (object sender, EventArgs e) => Debug.Log("Rewinding.........");
        onRewindStop += (object sender, EventArgs e) => Debug.Log("Rewind stopped");

        onRewindElementsAddStart += (object sender, EventArgs e) =>
        {
            Debug.Log("ElementsAdd Start");
            lastRewindElementsAddRealtime = realtime;

        };

        onRewindElementsAddStop += (object sender, EventArgs e) => Debug.Log("ElementsAdd Stop");
    }

    private void UpdateRealtime()
    {
        realtime = (int)Time.realtimeSinceStartup;
    }

    private bool CanAddRewindElements()
    {
        return (realtime % RewindElementsAddTimeThresold == 0) && (realtime != lastRewindElementsAddRealtime); 
    }

    private bool hasNotPassedSeconds(int currentRealtimeSinceStartup)
    {
        return realtime - currentRealtimeSinceStartup < RewindDurationInSeconds;
    }

    /// <summary>
    /// FIND REWINDABLE OBJECTS AND ADD THEM TO THE LIST
    /// </summary>
    private void UpdateRewindableObjects()
    {
        //var ss = FindObjectsOfType<MonoBehaviour>().OfType<IStats>();
        if (!isLive)
            rewindableObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IRewind>().ToArray();
    }


    /// <summary>
    /// USE THE CALLBACK DECLARED IN THE OBJECTS IMPLEMENTING IREWIND
    /// </summary>
    public void UpdateRewindElements()
    {
        if (CanAddRewindElements() && !isLive)
        {
            onRewindElementsAddStart?.Invoke(this, EventArgs.Empty);
            foreach (IRewind obj in rewindableObjects) obj?.UpdateRewindElements();
            Debug.Log("ADD ELEMENTS");
            onRewindElementsAddStop?.Invoke(this, EventArgs.Empty);
        }
            
    }

    /// <summary>
    /// START REWINDING
    /// </summary>
    private IEnumerator Rewind()
    {
            onRewindStart?.Invoke(this, EventArgs.Empty);
            int currentRealtimeSinceStartup = realtime;
            firstPersonController.PlayerCanMove = false;
            isLive = true;
            while (hasNotPassedSeconds(currentRealtimeSinceStartup)) //while (secodns in in-game time)
            {
                onRewindIteration?.Invoke(this, EventArgs.Empty);
                foreach (IRewind obj in rewindableObjects) obj?.Rewind();
                yield return null;
            }
            onRewindStop?.Invoke(this, EventArgs.Empty);
            firstPersonController.PlayerCanMove = true;
            isLive = false;
    }

    /// <summary>
    /// START REWINDING
    /// USE ALL OBJECTS IMPLEMENTING IREWIND
    /// </summary>
    private void Update()
    {
        UpdateRealtime();
        UpdateRewindableObjects();
        UpdateRewindElements();
        if (!isLive && Input.GetKeyDown(triggerKey))
            StartCoroutine(Rewind());
    }
}

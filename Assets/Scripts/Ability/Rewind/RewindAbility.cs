using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;


/// <summary>
/// EVENTATTRIBUTE OBJECT?
/// </summary>
public class RewindAbility : Ability
{
    [SerializeField]
    private IRewind[] rewindableObjects;
    public int RewindDurationInSeconds = 3;
    public int RewindElementsAddTimeThresold = 15; //new name?
    private int lastRewindElementsAddRealtime = 0;
    public GlobalStates GlobalStates = GlobalStates.Instance;

    public event EventHandler OnRewindStart;
    public event EventHandler OnRewindIteration;
    public event EventHandler OnRewindStop;
    public event EventHandler OnRewindElementsAddStart;
    public event EventHandler OnRewindElementsAddStop;

    public void Start()
    {
        OnRewindStart += (object sender, EventArgs e) => Debug.Log("Rewind started");
        OnRewindIteration += (object sender, EventArgs e) => Debug.Log("Rewinding.........");
        OnRewindStop += (object sender, EventArgs e) => Debug.Log("Rewind stopped");

        OnRewindElementsAddStart += (object sender, EventArgs e) =>
        {
            Debug.Log("ElementsAdd Start");
            lastRewindElementsAddRealtime = GlobalStates.Realtime;
        };

        OnRewindElementsAddStop += (object sender, EventArgs e) => Debug.Log("ElementsAdd Stop");
    }
    private bool CanAddRewindElements()
    {
        return (GlobalStates.Realtime % RewindElementsAddTimeThresold == 0) && (GlobalStates.Realtime != lastRewindElementsAddRealtime); 
    }

    private bool HasNotPassedSeconds(int currentRealtimeSinceStartup)
    {
        return GlobalStates.Realtime - currentRealtimeSinceStartup < RewindDurationInSeconds;
    }

    /// <summary>
    /// FIND REWINDABLE OBJECTS AND ADD THEM TO THE LIST
    /// </summary>
    private void UpdateRewindableObjects()
    {
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
            OnRewindElementsAddStart?.Invoke(this, EventArgs.Empty);
            foreach (IRewind obj in rewindableObjects) obj?.UpdateRewindElements();
            Debug.Log("ADD ELEMENTS");
            OnRewindElementsAddStop?.Invoke(this, EventArgs.Empty);
        }
            
    }

    /// <summary>
    /// START REWINDING
    /// </summary>
    private IEnumerator Rewind()
    {
        OnRewindStart?.Invoke(this, EventArgs.Empty);
        int currentRealtimeSinceStartup = GlobalStates.Realtime;
        firstPersonController.PlayerCanMove = false;
        isLive = true;
        while (HasNotPassedSeconds(currentRealtimeSinceStartup)) //while (secodns in in-game time)
        {
            OnRewindIteration?.Invoke(this, EventArgs.Empty);
            foreach (IRewind obj in rewindableObjects) obj?.Rewind();
            yield return null;
        }
        OnRewindStop?.Invoke(this, EventArgs.Empty);
        //GoOnCooldown()
        firstPersonController.PlayerCanMove = true;
        isLive = false;
    }

    /// <summary>
    /// START REWINDING
    /// USE ALL OBJECTS IMPLEMENTING IREWIND
    /// </summary>
    private void Update()
    {
        UpdateRewindableObjects();
        UpdateRewindElements();
        if (!isLive && Input.GetKeyDown(triggerKey) && !OnCooldown)
            StartCoroutine(Rewind());
    }
}

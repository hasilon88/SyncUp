using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;


/// <summary>
/// # OPTIMIZATIONS
/// - **TIMING SNAPSHOTTHRESHOLD WITH FLOATS**
/// - EVENTATTRIBUTE OBJECT?
/// - should keep a reference of RigidBody[] has attribute
/// - CAMERA REWINDER?
/// - should use .AddForce() while inverting Vector3s?
/// - GOONCOOLDOWN...
/// - STOP REWIND IN CERTAIN CONDITIONS
/// - STOP AUDIOMANAGER CAPTURE (LASTLOUDESTSAMPLES SNAPSHOT)
/// </summary>
public class RewindAbility : Ability
{
    private IRewind[] rewindableObjects;
    public int RewindDurationInSeconds = 3;
    public int SnapshotThresold = 1; 
    private int lastSnapshotTime = 0; 
    [Range(0f, 2f)]
    public float SecondsBetweenRewindIteration = 1f;
    public GlobalStates GlobalStates = GlobalStates.Instance;

    public event EventHandler OnRewindStart;
    public event EventHandler OnRewindIteration;
    public event EventHandler OnRewindStop;
    public event EventHandler OnRewindElementsAddStart;
    public event EventHandler OnRewindElementsAddStop;

    public void Start()
    {
        OnRewindStart += BeforeRewind;
        OnRewindIteration += (object sender, EventArgs e) => Debug.Log("Rewinding.........");
        OnRewindStop += AfterRewind;

        OnRewindElementsAddStart += (object sender, EventArgs e) =>
        {
            Debug.Log("ElementsAdd Start");
            lastSnapshotTime = GlobalStates.Realtime;
        };

        OnRewindElementsAddStop += (object sender, EventArgs e) => 
        {
            Debug.Log("ElementsAdd Stop");
        };

    }

    private void BeforeRewind(object sender, EventArgs e)
    {
        Debug.Log("Rewind start");
        PrepareRigidBodies();
        firstPersonController.PlayerCanMove = false; //enemy can move?
        isLive = true;
    }

    private void AfterRewind(object sender, EventArgs e)
    {
        Debug.Log("Rewind stopped");
        firstPersonController.PlayerCanMove = true;
        isLive = false;
        UnPrepareRigidBodies();
        //GoOnCooldown()
    }

    private bool CanAddRewindElements()
    {
        if (SnapshotThresold > 0)
            return (GlobalStates.Realtime % SnapshotThresold == 0) && (GlobalStates.Realtime != lastSnapshotTime);
        else return true;
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

    private Rigidbody[] GetRigidBodies()
    {
        return GameObject.FindObjectsOfType<Rigidbody>().ToArray();
    }

    private void PrepareRigidBodies()
    {
        foreach (Rigidbody body in FindObjectsOfType<Rigidbody>().ToArray())
        {
            body.useGravity = false;
            body.velocity = Vector3.zero;
            //body.angularVelocity = Vector3.zero; //CAMERA REWINDER?
        }
    }

    private void UnPrepareRigidBodies()
    {
        foreach (Rigidbody body in FindObjectsOfType<Rigidbody>().ToArray())
            body.useGravity = true;
    }

    /// <summary>
    /// START REWINDING
    /// </summary>
    private IEnumerator Rewind()
    {
        OnRewindStart?.Invoke(this, EventArgs.Empty);
        int currentRealtimeSinceStartup = GlobalStates.Realtime;
        while (HasNotPassedSeconds(currentRealtimeSinceStartup)) //while (secodns in in-game time)
        {
            OnRewindIteration?.Invoke(this, EventArgs.Empty);
            foreach (IRewind obj in rewindableObjects) obj?.Rewind();
            yield return new WaitForSeconds(SecondsBetweenRewindIteration);
        }
        OnRewindStop?.Invoke(this, EventArgs.Empty);
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

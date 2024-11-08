using System.Collections;
using System;
using UnityEngine;
using System.Linq;


/// <summary>
/// # OPTIMIZATIONS
/// - **TIMING SNAPSHOTTHRESHOLD WITH FLOATS**
/// - EVENTATTRIBUTE OBJECT?
/// - should use .AddForce() while inverting Vector3s?
/// - GOONCOOLDOWN...
/// - STOP AUDIOMANAGER CAPTURE (LASTLOUDESTSAMPLES SNAPSHOT)
/// - DURING REWIND, EXPAN FIELD OF VIEW
/// - HIGH CONTRAST ON REWIND
/// - BLACK TUNNEL VISION
/// - WIND TUNNEL
/// - CREATE TIMESNAPSHOT CLASS
/// 
/// - Will search for objects implementing IRewind
/// to then use UpdateRewindElements() every x seconds,
/// thereby adding previous elements (Vector3, Color, ...).
/// - WIll use Rewind of every object implementing IRewind
/// to use there custom implementation
/// 
/// </summary>
public class RewindAbility : Ability
{
    private IRewind[] rewindableObjects;
    private Rigidbody[] rewindableRigidbodies;
    public int RewindDurationInSeconds = 3;
    public int SnapshotThresold = 1; 
    private int lastTimeSnapshot = 0; 
    [Range(0f, 2f)]
    public float TargetRewindIterationDelay = 0.5f;
    public float TargetRewindIterationFOV = 120f;
    private Vector2[] iterationDelays;
    private Vector2[] iterationFOVs;
    private GlobalStates globalStates;

    public event EventHandler OnRewindStart;
    public event EventHandler OnRewindIteration;
    public event EventHandler OnRewindStop;
    public event EventHandler OnRewindElementsAddStart;
    public event EventHandler OnRewindElementsAddStop;

    public void Start()
    {
        globalStates = GlobalStates.Instance;
        OnRewindStart += BeforeRewind;
        //OnRewindIteration += (object sender, EventArgs e) => Debug.Log("Rewinding.........");
        OnRewindStop += AfterRewind;
        OnRewindElementsAddStart += (object sender, EventArgs e) => lastTimeSnapshot = globalStates.ScaledTime;
    }

    private void BeforeRewind(object sender, EventArgs e)
    {
        rewindableRigidbodies = FindObjectsOfType<Rigidbody>().ToArray();
        //iterationDelays = ParabolicArray.GetArray(TargetRewindIterationDelay, ((60 * RewindDurationInSeconds) / 2)); //* UpdateCountPerSecond
        //iterationFOVs = ParabolicArray.GetArray(TargetRewindIterationFOV, ((60 * RewindDurationInSeconds) / 2)); //* UpdateCountPerSecond
        PrepareRigidBodies();
        firstPersonController.PlayerCanMove = false; //enemy can move?
        isLive = true;
    }

    private void AfterRewind(object sender, EventArgs e)
    {
        firstPersonController.PlayerCanMove = true;
        isLive = false;
        UnPrepareRigidBodies();
        //GoOnCooldown()
    }

    private bool CanAddRewindElements()
    {
        if (SnapshotThresold > 0)
            return (globalStates.ScaledTime % SnapshotThresold == 0) && (globalStates.ScaledTime != lastTimeSnapshot);
        else return true;
    }

    /// <summary>
    /// ==========> BUG: IF GLOBAL REALTIME IS BELLOW DURATION AT START <==========
    /// shouldn't be a problem in the actual game
    /// </summary>
    private bool HasNotPassedSeconds(int currentRealtimeSinceStartup)
    {
        if (currentRealtimeSinceStartup < 0) return false;
        return (globalStates.ScaledTime - currentRealtimeSinceStartup < RewindDurationInSeconds);
    }

    /// <summary>
    /// FIND REWINDABLE OBJECTS AND ADD THEM TO THE LIST
    /// - SHOULD BE ONLY WHEN A NEW IREWIND OBJECT IS CREATED
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
            OnRewindElementsAddStop?.Invoke(this, EventArgs.Empty);
        }
    }

    private void PrepareRigidBodies()
    {
        foreach (Rigidbody body in rewindableRigidbodies)
        {
            body.useGravity = false;
            body.velocity = Vector3.zero;
        }
    }

    private void UnPrepareRigidBodies()
    {
        foreach (Rigidbody body in rewindableRigidbodies)
            body.useGravity = true;
    }

    /// <summary>
    /// START REWINDING
    /// - AFTER IMAGE
    /// - SLOW - FAST - SLOW (SecondsBetweenRewindIteration)
    /// </summary>
    private IEnumerator Rewind()
    {
        OnRewindStart?.Invoke(this, EventArgs.Empty);
        int currentRealtimeSinceStartup = globalStates.ScaledTime;
        RewindResponse res;
        while (HasNotPassedSeconds(currentRealtimeSinceStartup))
        {
            OnRewindIteration?.Invoke(this, EventArgs.Empty);
            for (int elem = 0; elem < rewindableObjects.Length; elem++)
            {
                res = rewindableObjects[elem].Rewind();
                if (res.HasToStop && res.RewindingObject.CompareTag("Player"))
                {
                    currentRealtimeSinceStartup = -1;
                    break;
                }
            }
            yield return new WaitForSeconds(TargetRewindIterationDelay);
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

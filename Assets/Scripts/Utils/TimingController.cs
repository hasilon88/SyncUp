using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public enum TimeType
{
    REALTIME,
    SCALEDTIME
}

public class TimingController : MonoBehaviour
{


    public float Target = 1f;
    public TimeType Type = TimeType.SCALEDTIME;
    private float timeSnapshot = 0f;
    private GlobalStates globalStates;
    public event EventHandler OnTime;
    public bool IsOnTime = false;

    /// <summary>
    /// TYPE = REALTIME...FOR PRECISE TIMING
    /// </summary>
    private void Start()
    {
        globalStates = GlobalStates.Instance;
        OnTime += (object sender, EventArgs e) => 
        {
            IsOnTime = true;
            SetSnapShot(Type, out timeSnapshot);
        };
    }

    private static void SetSnapShot(TimeType type, out float value)
    {
        switch(type)
        {
            case TimeType.REALTIME:
                value = GlobalStates.Instance.RealTime;
                break;
            case TimeType.SCALEDTIME:
                value = GlobalStates.Instance.ScaledTime;
                break;
            default:
                value = GlobalStates.Instance.RealTime;
                break;
        }
    }

    private bool HasPassed(TimeType type)
    {
        if (type == TimeType.SCALEDTIME)
            return globalStates.ScaledTime - timeSnapshot >= Target;
        else if (type == TimeType.REALTIME)
            return globalStates.RealTime - timeSnapshot >= Target;
        else throw new Exception("Error in Timer.cs.....");
    }

    private void Update()
    {
        if (HasPassed(Type))
            OnTime?.Invoke(this, EventArgs.Empty);
        else IsOnTime = false;
    }

    public static IEnumerator Time(TimeType type, float seconds, Action callback) //out CooldownCountdown <==
    {
        SetSnapShot(type, out float snapShot);        
        if (type == TimeType.REALTIME)
            while (GlobalStates.Instance.RealTime - snapShot <= seconds)
                yield return null;
        else if (type == TimeType.SCALEDTIME)
            while (GlobalStates.Instance.ScaledTime - snapShot <= seconds)
                yield return null;
        callback();
    }

}

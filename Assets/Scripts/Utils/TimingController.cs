using System;
using Unity.VisualScripting;
using UnityEngine;

public class TimingController : MonoBehaviour
{

    public enum TimeType
    {
        REALTIME,
        SCALEDTIME
    }

    public float Target = 1f;
    public TimeType Type = TimeType.SCALEDTIME;
    private float timeSnapshot = 0f;
    private GlobalStates globalStates;
    public event EventHandler OnTime;
    public bool IsOnTime = false;

    private void Start()
    {
        globalStates = GlobalStates.Instance;
        OnTime += (object sender, EventArgs e) => 
        {
            IsOnTime = true;
            if (Type == TimeType.SCALEDTIME) timeSnapshot = globalStates.ScaledTime;
            else if (Type == TimeType.REALTIME) timeSnapshot = globalStates.RealTime;
        };
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


}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;

public class MultiBullets : MonoBehaviour
{

    public float InitialElementSize = 1;
    public int SubsequentElementCount = 2;
    public float ElementsSpacing = 1f;
    private GameObject[] elements;

    [Range(0f, 1f)]
    public float SubsequentElementsExpansionRate = 2f;
    public float TemporalExpandingRateInSeconds = 1; // each x milliseconds

    public bool IsTraveling = false;
    public float TravelSpeed = 5f;
    public Vector3 Angle;

    [Range(0f, 1f)]
    public float SizeExpandingRate = 0.10f; //Normalized

    public float LifespanInSeconds = 60f;
    public bool Dissipates = true;
    public bool IsDissipating = false;

    public GameObject BaseModel;

    private void Awake()
    {
        name = "SoundBullet";
        throw new NotImplementedException();
    }

    private void Start()
    {
        StartCoroutine(TimingController.Time(TimeType.SCALEDTIME, LifespanInSeconds, () => StartCoroutine(Dissipate())));
        
        throw new NotImplementedException();
    }

    private void Iterate(Action<GameObject> callback)
    {
        foreach (GameObject obj in elements)
        {
            callback(obj);
        }
    }

    /// <summary>
    /// for (SubsequentELementCount)
    /// </summary>
    private void InstantiateSubsequentElements()
    {
        throw new NotImplementedException();
    }

    private void Expand()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// if (IsTraveling)
    /// </summary>
    private void Travel()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// - Destroy(this);
    /// - Fadeaway animation
    /// </summary>
    private IEnumerator Dissipate()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        Travel();
    }

}

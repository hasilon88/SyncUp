using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SoundBullet : MonoBehaviour
{

    public float InitialElementScale = 0.50f;
    public int ElementCount = 3;
    [Range(1f, 2f)]
    public float SubsequentElementsSpacing = 1.2f;
    [Range(0f, 1f)]
    public float SubsequentElementsExpansionRate = 1f;
    public float TemporalExpandingRateInSeconds = 1; // each x milliseconds
    private GameObject[] elements;

    [Range(0f, 1f)]
    public float SizeExpandingRate = 0.10f; //Normalized
    public float TravelSpeed = 0.30f;
    private Vector3 direction;
    public float LifespanInSeconds = 5f;

    public bool CanTravel = false;
    public bool Dissipates = true;

    private Rigidbody _rigidbody;
    private GameObject baseBulletModel;

    //public bool Bounces?
    //public bool Richochet?

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(TimingController.Time(TimeType.SCALEDTIME, LifespanInSeconds, () => StartCoroutine(Dissipate())));
    }

    public void SetBaseBulletModel(GameObject baseModel)
    {
        baseBulletModel = baseModel;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void InstantiateElements()
    {
        Vector3 size = Vector3.one * InitialElementScale;
        Vector3 position = gameObject.transform.position;
        elements = new GameObject[ElementCount];
        for (int elem = 0; elem < ElementCount; elem++)
        {
            Debug.Log(position);
            GameObject segment = Instantiate(baseBulletModel, position, Quaternion.Euler(Vector3.zero), gameObject.transform);
            //segment.transform.localScale = size;
            //segment.transform.rotation = Quaternion.Euler(direction);
            //size = size + (size * SubsequentElementsExpansionRate);
            position = position * SubsequentElementsSpacing;
            elements[elem] = segment;
        }
    }

    //gameObject, RigidBodies, VertexColor
    private void Iterate(Action<GameObject> callback)
    {
        foreach (GameObject obj in elements)
            callback(obj);
    }


    //private void Expand()
    //{
    //    throw new NotImplementedException();
    //}

    public void StartTravelling()
    {
        CanTravel = true;
    }

    private void Travel()
    {
        if (CanTravel)
        {
            Debug.Log(direction.normalized + " || " + TravelSpeed * Time.deltaTime * direction);
            _rigidbody.AddForce(TravelSpeed * Time.deltaTime * direction);
        }
    }

    /// <summary>
    /// - Destroy(this);
    /// - Fadeaway animation
    /// </summary>
    private IEnumerator Dissipate()
    {
        //fading
        Debug.Log("DEATH?");
        Destroy(gameObject);
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        Travel();
    }

}

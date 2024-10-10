using UnityEngine;
using UnityEngine.UIElements;

public class PlatformElevator : MonoBehaviour
{

    public int ElevationLimitOffSet = 3;
    public int IncrementValue = 1;
    public bool IsIncrementing = true;
    public float FrequencyMultiplier = 1.5f;
    private Vector3 InitialPosition;
    public AudioManager AudioManager;

    void Start()
    {
        this.InitialPosition = this.transform.position;
        
    }

    //public void ResetPosition()
    //{
    //    this.transform.position = new Vector3(this.InitialPosition.x, 0, 0);
    //}

    //private void SetIsIncrementing()
    //{
    //    if (this.transform.position.y >= this.InitialPosition.y + this.ElevationLimitOffSet)
    //        this.IsIncrementing = false;
    //    else if (this.transform.position.y <= this.InitialPosition.y  - this.ElevationLimitOffSet)
    //        this.IsIncrementing = true;
    //}

    //private void ChangeY()
    //{
    //    float value = this.IncrementValue * Time.deltaTime;
    //    float multiplier = (1 + (this.AudioManager.CurrentToMaxFrequencyPercentage/100f)) * this.FrequencyMultiplier 
    //        * this.AudioManager.CurrentToMaxFrequencyPercentage <= 0f ? 0f : 1f;

    //    if (this.IsIncrementing) this.transform.position += new Vector3(0, value * multiplier, 0);
    //    else this.transform.position -= new Vector3(0, value * multiplier, 0);
    //}

    //void Update()
    //{
    //    this.SetIsIncrementing();
    //    this.ChangeY();
    //}
}

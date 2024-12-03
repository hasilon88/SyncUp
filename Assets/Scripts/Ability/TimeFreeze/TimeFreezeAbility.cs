using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TimeFreeze : Ability
{
    [Header("Technical Settings")]
    public Camera Camera;
    public PostProcessProfile Profile;
    public float MaxVignetteIntensity;
    [Header("Ability Settings")]    
    public int AbilityDurationSeconds;
    private PostProcessVolume PostProcessVolume;
    private AudioManager AudioManager;

    private EventHandler UnFreezeEvent;
    private EventHandler FreezeEvent;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager = AudioManager.Instance;
        AbilityDurationSeconds = 5;
        this.PostProcessVolume = Camera.GetComponent<PostProcessVolume>();
        FreezeEvent += (object Sender, EventArgs e) => { AudioManager.Frozen = true; };
        UnFreezeEvent += (object Sender, EventArgs e) => { AudioManager.Frozen = false; };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("PRESSED C");
            StartCoroutine(Freeze());
        }
    }

    private IEnumerator Freeze()
    {
        Debug.Log("Freezing");
        FreezeEvent?.Invoke(this, EventArgs.Empty);
        while (PostProcessVolume.weight <= 1)
        {
            PostProcessVolume.weight += 0.01f;
            yield return new WaitForSeconds(0.1f * Time.deltaTime);
        }

        StartCoroutine(TimingController.Time(TimeType.REALTIME, AbilityDurationSeconds, () =>
        {
            StartCoroutine(UnFreeze());
        }));
    }

    private IEnumerator UnFreeze()
    {
        UnFreezeEvent?.Invoke(this, EventArgs.Empty);
        PostProcessVolume.weight = 1;
        Debug.Log("Un-Freezing");
        while (PostProcessVolume.weight > 0)
        {
            PostProcessVolume.weight -= 0.01f;
            yield return new WaitForSeconds(0.1f * Time.deltaTime);
        }
        PostProcessVolume.weight = 0;
    }
}
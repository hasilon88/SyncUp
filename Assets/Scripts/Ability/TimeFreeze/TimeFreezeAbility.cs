using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TimeFreezeAbility : Ability
{

    private AudioManager audioManager = AudioManager.Instance;
    public PostProcessVolume PostProcessVolume;
    private ColorGrading colorGrading;

    private void Start()
    {
        colorGrading = (ColorGrading)PostProcessVolume.sharedProfile.settings[0];
    }

    private IEnumerator FreezeTime()
    {
        yield return null;
    }

    void Update()
    {
        colorGrading.saturation.value = -100f * audioManager.NormalizedCurrentLoudestSample_LastLoudestSamplesMax;
        //if (!IsLive && Input.GetKeyDown(TriggerKey) && !OnCooldown)
        //    StartCoroutine(FreezeTime());
    }
}

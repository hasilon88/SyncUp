using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{

    [Range(0f, 15f)]
    public float CooldownInSeconds = 3f;
    public float CooldownCountdown = 0f; //todo
    public bool CooldownEnabled = true;
    public bool OnCooldown = false;
    public string Name;
    public float Duration = 0.8f;
    public Image Icon; 
    public bool IsLive = false;
    public KeyCode TriggerKey;
    protected PlayerController PlayerController;

    public event EventHandler OnCooldownEnter;
    public event EventHandler OnCooldownLeave;

    public void GoOnCooldown()
    {
        OnCooldownEnter?.Invoke(this, EventArgs.Empty);
        OnCooldown = true;
        StartCoroutine(TimingController.Time(TimeType.SCALEDTIME, CooldownInSeconds, ExitOnCooldown));
    }

    public void ExitOnCooldown()
    {
        OnCooldownLeave?.Invoke(this, EventArgs.Empty);
        OnCooldown = false;
    }


}

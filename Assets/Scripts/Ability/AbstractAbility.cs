using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{

    [Range(0f, 15f)]
    public float Cooldown = 3; //cooldown value (seconds)
    public float CooldownCountdown = 0;
    public bool CooldownEnable = true; //
    protected bool OnCooldown { get; private set; }
    public string Name; //name of ability in the store
    public Image Icon; //icon to be shown
    public bool isLive = false; //if ablity is currently being used (has been fired) //could be disregarded
    public int CreditsNeeded = 0; //price in the store
    public KeyCode triggerKey;
    public FirstPersonController firstPersonController;

    public event EventHandler OnAbilityEnabled;
    public event EventHandler OnAbilityDisabled;
    public event EventHandler OnCooldownEnter;
    public event EventHandler OnCooldownLeave;

    public void EnableAbility()
    {
        gameObject.SetActive(true);
        OnAbilityEnabled?.Invoke(this, EventArgs.Empty);

    }

    public void DisableAbility()
    {
        gameObject.SetActive(false);
        OnAbilityDisabled?.Invoke(this, EventArgs.Empty);
    }

    public void GoOnCooldown()
    {
        OnCooldown = true;
        OnCooldownEnter?.Invoke(this, EventArgs.Empty);

    }

    public void LeaveOnCooldown()
    {
        OnCooldown = false;
        OnCooldownLeave?.Invoke(this, EventArgs.Empty);
    }

}

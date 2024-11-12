using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{

    [Range(0f, 15f)]
    public float CooldownInSeconds = 3f;
    private int cooldownInFrames = 0;
    public float CooldownCountdown = 0f;
    public bool CooldownEnabled = true;
    public bool OnCooldown = false;

    public string Name; 
    public Image Icon; 
    public bool IsLive = false;
    public bool IsEquipped = false;
    public bool IsUnlocked = false;
    public int CreditsNeeded = 0; 
    public KeyCode TriggerKey;
    public FirstPersonController FirstPersonController;

    public event EventHandler OnAbilityUnlock;
    public event EventHandler OnAbilityEquip;
    public event EventHandler OnAbilityUnequip;
    public event EventHandler OnCooldownEnter;
    public event EventHandler OnCooldownLeave;

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

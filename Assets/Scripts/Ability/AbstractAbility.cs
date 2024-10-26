using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{

    [Range(0f, 15f)]
    public float Cooldown; //cooldown value
    private bool onCooldown; //if is on cooldown
    public string Name; //name of ability in the store
    public Image Icon; //icon to be shown
    public bool isLive; //if ablity is currently being used (has been fired) //could be disregarded
    public int CreditsNeeded; //price in the store
    public KeyCode triggerKey;

    public event EventHandler OnAbilityEnabled;

    public void EnableAbility()
    {
        this.gameObject.SetActive(true);
        this.OnAbilityEnabled(this, EventArgs.Empty);

    }

    public void DisableAbility()
    {
        this.gameObject.SetActive(false);
    }

    public void GoOnCooldown()
    {

    }

    public void SetOnCooldown()
    {

    }


}

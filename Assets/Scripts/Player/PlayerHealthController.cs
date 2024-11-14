using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{

    public float MaxHealth = 100f;
    public float CurrentHealth;
    public bool CanRegenerate = false;
    public bool IsInvulnerable = false;
    public float HealthAlterationSpeed = 10f;
    public float HealthAlterationPerFrames = 1f;
    public Slider HealthSlider;

    void Start()
    {
        CurrentHealth = MaxHealth;
        //getSlider
    }

    private IEnumerator ModifyHealth(float by)
    {
       
        yield return new WaitForSeconds(HealthAlterationSpeed * Time.deltaTime);
    }

    public void ReduceHealth()
    {

    }

    public void RegenerateHealth()
    {

    }

    private void Update()
    {
        
    }

}

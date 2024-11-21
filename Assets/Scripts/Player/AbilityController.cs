using UnityEngine;

public enum Abilities
{
    NONE,
    REWIND,
    SOUND_WAVE
}


public class AbilityController : MonoBehaviour
{

    private GlobalStates globalStates;
    private GameObject abilitySource;
    //private PlayerController playerController;
    public GameObject RewindPrefab;

    private void Start()
    {
        globalStates = GlobalStates.Instance;
        abilitySource = GameObject.FindGameObjectWithTag("AbilitySource");
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        SetFirstEquippedAbilities();
    }

    private void SetFirstEquippedAbilities()
    {
        if (globalStates.FirstAbility == Abilities.REWIND) 
        {
            var ins = Instantiate(RewindPrefab, abilitySource.transform);
        }
    }



}

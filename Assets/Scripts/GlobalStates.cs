using UnityEngine;


/// <summary>
/// SINGLETON FOR GLOBAL VARIABLES
/// GlobalStates s = GlobalStates.Instance;
/// </summary>
public class GlobalStates : MonoBehaviour
{

    public static GlobalStates Instance { get; private set; }
    public Ability Ability1 {  get; set; }  
    public Ability Ability2 {  get; set; }
    public int Credits;
    public int CurrentLevel = 1;

    private void Awake()
    {
        if (Instance == null || Instance != this)
            Destroy(this);
        else Instance = this;
    }

}

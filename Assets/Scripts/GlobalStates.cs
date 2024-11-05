using UnityEngine;

/// <summary>
/// SINGLETON FOR GLOBAL VARIABLES
/// GlobalStates s = GlobalStates.Instance;
/// </summary>
public class GlobalStates : MonoBehaviour
{

    public static GlobalStates Instance { get; private set; }
    public Ability Ability1 = null;
    public Ability Ability2 = null;
    public int Credits = 0;
    public int CurrentLevel = 1;
    public int Realtime = 0;
    public int RewindArrayLength = 50;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
        //on loading a new scene, object wont be destroyed
        DontDestroyOnLoad(this);
    }

    private void UpdateRealtime()
    {
        Realtime = (int)Time.time;
    }

    private void Update()
    {
        UpdateRealtime();
    }

}

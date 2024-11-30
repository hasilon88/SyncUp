using UnityEngine;

/// <summary>
/// SINGLETON FOR GLOBAL VARIABLES
/// GlobalStates s = GlobalStates.Instance;
/// </summary>
public class GlobalStates : MonoBehaviour
{

    public static GlobalStates Instance;
    public int Credits = 0;
    public int CurrentLevel = 1;
    public int ScaledTime = 0;
    public float RealTime = 0f;
    public Abilities FirstAbility;
    public Abilities SecondAbility;
    public bool DebugMode = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }

    private void UpdateTime()
    {
        ScaledTime = (int)Time.time;
        RealTime = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        UpdateTime();
    }

}

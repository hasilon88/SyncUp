using UnityEngine;

/// <summary>
/// Each Scene (LEVEL) will have a root gameObject with a script extending Level
/// </summary>
public abstract class Level : MonoBehaviour
{

    public int LevelID;
    public string Name;
    public Challenge[] challenges;
    
}

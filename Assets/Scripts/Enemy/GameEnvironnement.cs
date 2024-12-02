using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameEnvironment
{

    public static GameEnvironment Instance;
    public List<GameObject> Checkpoints = new List<GameObject>();

    public static GameEnvironment GetInstance()
    {
        if (Instance == null)
        {
            Instance = new GameEnvironment();
            Instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint").OrderBy(waypoint => waypoint.name).ToList());
        }
        return Instance;
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syncer : MonoBehaviour
{
    public Renderer Renderer;
    public ColorSync ColorSync;

    void Start()
    {
        Renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        Renderer.material.color = ColorSync.CurrentColor;
    }
}

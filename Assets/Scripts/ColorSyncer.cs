using UnityEngine;

public class ColorSyncer : MonoBehaviour
{
    private Renderer Renderer;
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

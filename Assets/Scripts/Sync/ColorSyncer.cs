using UnityEngine;

public class ColorSyncer : MonoBehaviour
{

    private Renderer _renderer;
    public ColorSync ColorSync;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        _renderer.material.color = ColorSync.CurrentColor;
    }
}

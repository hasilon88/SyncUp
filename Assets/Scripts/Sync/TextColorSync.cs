using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextColorSync : MonoBehaviour
{

    private TextMeshProUGUI text;
    public ColorSync colorSync;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.color = colorSync.CurrentColor;
        text.alpha = 1f;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempoIndicatorBehavior : MonoBehaviour
{

    private Image _image;
    private TempoSetter tempoSetter;

    void Start()
    {
        _image = GetComponent<Image>();
        tempoSetter = GameObject.Find("AudioManager").GetComponent<TempoSetter>();
        tempoSetter.OnBeat += (object sender, EventArgs e) => { _image.color = Color.red; };
        tempoSetter.OnBeatLeave += (object sender, EventArgs e) => { _image.color = Color.white; };
    }

}

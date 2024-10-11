using UnityEngine;
using System;

public class ColorSync : SyncObject
{

    public Color CurrentColor;
    public Color[] ColorSpectrum;

    private Color GetAdaptivePercentageBasedSampleColor()
    {
        if (this.ColorSpectrum == null || ColorSpectrum.Length == 0) this.ColorSpectrum = ColorSpectrums.DEFAULT_SPECTRUM;
        Color nColor = this.ColorSpectrum[0];
        float currentToMaxAverageFrequencyPercentage = this.AudioManager.NormalizedCurrentLoudestSample_LastLoudestSamplesMax * 100f;
        float partPercentage = 100f / this.ColorSpectrum.Length;
        float percentageDifference;
        float nextPercentageDifference;
        for (int elem = 0; elem < this.ColorSpectrum.Length - 1; elem++)
        {
            percentageDifference = currentToMaxAverageFrequencyPercentage - (partPercentage * (elem + 1));
            nextPercentageDifference = currentToMaxAverageFrequencyPercentage - (partPercentage * (elem + 2));
            if (nextPercentageDifference < percentageDifference && nextPercentageDifference > 0) nColor = this.ColorSpectrum[elem + 1];
            else if (nextPercentageDifference < 0) break; //if difference is negative, then the actual closest percentage has already passed
        }
        return nColor;
    }

    public override void Sync()
    {
        this.CurrentColor = GetAdaptivePercentageBasedSampleColor();
    }
}
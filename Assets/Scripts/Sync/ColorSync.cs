using UnityEngine;

public class ColorSync : SyncObject
{

    public Color CurrentColor;
    public Color[] ColorSpectrum;
    public float Alpha = 1f;

    private Color GetAdaptivePercentageBasedSampleColor()
    {
        if (ColorSpectrum == null || ColorSpectrum.Length < 3) this.ColorSpectrum = ColorSpectrums.DEFAULT_SPECTRUM; //needs to be changed for somethnig else
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
        return new Color(nColor.r, nColor.g, nColor.b, Alpha);
    }

    public override void Sync()
    {
        this.CurrentColor = GetAdaptivePercentageBasedSampleColor();
    }
}
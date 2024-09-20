using UnityEngine;

public class ColorSync : MonoBehaviour
{

    [Range(1, 32)]
    public byte Tempo = 4;
    public AudioManager AudioManager;
    public Color CurrentColor;
    public Color[] ColorsRange;

    private Color GetAdaptivePercentageBasedSampleColor(float currentMaxFrequency, float averageMaxFrequency, float averageFrequency, params Color[] colors)
    {
        if (colors == null || colors.Length == 0) colors = new Color[3] { Color.white, Color.black, Color.red };
        Color nColor = colors[0];
        float currentToMaxAverageFrequencyPercentage = (currentMaxFrequency * 100f) / averageMaxFrequency;
        float partPercentage = 100f / colors.Length;
        float percentageDifference;
        float nextPercentageDifference;
        float closestPercentageDifference = 1f;
        for (int elem = 0; elem < colors.Length - 1; elem++)
        {
            percentageDifference = currentToMaxAverageFrequencyPercentage - (partPercentage * (elem + 1));
            nextPercentageDifference = currentToMaxAverageFrequencyPercentage - (partPercentage * (elem + 2));
            if (nextPercentageDifference < percentageDifference && nextPercentageDifference > 0)
            {
                nColor = colors[elem + 1];
                closestPercentageDifference = nextPercentageDifference;
            }
            else if (nextPercentageDifference < 0) break; //if difference is negative, then the actual closest percentage has already passed
        }
        return AdjustSampleColorToDifference(nColor, closestPercentageDifference);
    }

    private Color AdjustSampleColorToDifference(Color color, float diff)
    {
        diff = diff / 100f;
        color.r = color.r + (color.r * diff);
        color.g = color.g + (color.g * diff);
        color.b = color.b + (color.b * diff);
        return color;
    }

    void Update()
    {
        this.CurrentColor =
            this.GetAdaptivePercentageBasedSampleColor(
                        this.AudioManager.CurrentMaxFrequency,
                        this.AudioManager.AverageMaxSampleFrequency,
                        this.AudioManager.AverageSampleFrequency,
                        this.ColorsRange);
    }
}
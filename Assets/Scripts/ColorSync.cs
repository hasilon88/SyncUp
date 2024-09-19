using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorSync : MonoBehaviour
{

    public AudioManager AudioManager;
    public byte Tempo = 4;
    public Color CurrentColor;
    public Color[] ColorsRange;

    private Dictionary<float, Color> GetColorPercentageDictionary(Color[] colors)
    {
        Dictionary<float, Color> colorsPercentages = new Dictionary<float, Color>();
        float percentagePerParts = 100f/colors.Length;
        float individualPercentage = 0f;
        foreach (Color c in colors)
        {
            individualPercentage += percentagePerParts;
            colorsPercentages.Add(individualPercentage, c);
        }
        return colorsPercentages;
    }

    public Color GetAdaptivePercentageBasedSampleColor(float currentMaxFrequency, float averageMaxFrequency, float averageFrequency, params Color[] colors)
    {
        if (colors == null || colors.Length == 0) colors = new Color[3] { Color.white, Color.yellow, Color.red };
        //getting percentage representation of current frequency to averageMaxFrequency
        //if currentMaxFrequency = 0.13 and averageMaxFrequency = 0.33 then percentage = 39.393939...%
        float currentToMaxAverageFrequencyPercentage = (currentMaxFrequency * 100)/averageMaxFrequency;
        Dictionary<float, Color> colorsPercentages = GetColorPercentageDictionary(colors);
        Color nColor = colorsPercentages.First().Value;
        //temporary variable to find the closest percentage to [currentToMaxAverageFrequencyPercentage]
        float tempDiff = currentToMaxAverageFrequencyPercentage - colorsPercentages.First().Key;
        foreach (KeyValuePair<float, Color> value in colorsPercentages)
            if (((currentToMaxAverageFrequencyPercentage - value.Key) < tempDiff) && (currentToMaxAverageFrequencyPercentage - value.Key > 0))
            {
                tempDiff = value.Key;
                nColor = value.Value;
            }
        //taking the difference in percentages from the right [colorsPercentages] to [currentToMaxAverageFrequencyPercentage] to get a more accurate color 
        return AdjustSampleColorToDifference(nColor, tempDiff);
    }

    public Color GetPercentageBasedSampleColor(float currentMaxFrequency, params Color[] colors)
    {
        if (colors == null || colors.Length == 0) colors = new Color[3] { Color.white, Color.yellow, Color.red };
        float currentMaxFrequencyPercentage = currentMaxFrequency * 100;
        Dictionary<float, Color> colorsPercentages = GetColorPercentageDictionary(colors);
        Color nColor = colorsPercentages.First().Value;
        float tempDiff = currentMaxFrequencyPercentage - colorsPercentages.First().Key;
        foreach (KeyValuePair<float, Color> value in colorsPercentages)
            if (((currentMaxFrequencyPercentage - value.Key) < tempDiff) && (currentMaxFrequencyPercentage - value.Key > 0))
            {
                tempDiff = value.Key;
                nColor = value.Value;
            }
        return AdjustSampleColorToDifference(nColor, tempDiff);
    }

    private Color AdjustSampleColorToDifference(Color color, float diff)
    {
        diff = diff/100f;
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

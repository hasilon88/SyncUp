using UnityEngine;


/// <summary>
/// 
/// @Unity Color spectrum reference points (UPPER RIGHT CORNER)
/// a (0.5, 1, 0)
/// b (1, 0.75, 0)
/// c (1, 0, 0)
/// d (1, 0, 0.75)
/// e (0.5, 0, 1)
/// f (0, 0.25, 1)
/// g (0, 1, 1)
/// h (0, 1, 0.25)
/// 
/// @Relation between segments
/// a -> b [r+0.5, g-0.25]
/// b -> c [g-0.75]
/// c -> d [b+0.75]
/// d -> e [r-0.5, b+0.25]
/// e -> f [r-0.5, g+0.25]
/// f -> g [g+0.75]
/// g -> h [b-0.75]
/// h -> a [r+0.5,b-0.25]
/// </summary>
public class ColorChainedInterpolator
{

    private ColorNode firstColorNode;

    public ColorChainedInterpolator(params Color[] colors) 
    { 
        if (colors.Length >= 8)
            this.SetColorNodeChain(colors); 
    }

    private void SetColorNodeChain(params Color[] colors)
    {
        this.firstColorNode = new ColorNode(colors[0]);
        ColorNode currentColorNode = this.firstColorNode;
        for (int elem = 1; elem < colors.Length; elem++) 
        { 
            ColorNode nColorNode = new ColorNode(colors[elem]);
            currentColorNode.Next = nColorNode;
            nColorNode.Previous = currentColorNode;
            currentColorNode = nColorNode;
        }
        currentColorNode.Next = this.firstColorNode;
        this.firstColorNode.Previous = currentColorNode;
    }

    public int GetChainLength()
    {
        ColorNode tempColorNode = this.firstColorNode;
        int chainLength = 0;
        while (tempColorNode != this.firstColorNode || chainLength == 0)
        {
            tempColorNode = tempColorNode.Next;
            chainLength++;
        }
        return chainLength;
    }

    public Color[] GetColorSpectrum(int spectrumLength, Color startColor, Color endColor)
    {
        float percentagePart = 100f / spectrumLength;
        float currentPercentage = 0f;

        return null;
    }
    
}
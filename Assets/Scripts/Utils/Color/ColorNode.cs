using UnityEngine;

public class ColorNode
{
   
    public Color Color;
    public ColorNode Next { get; private set; }
    public ColorNode Previous;
    public float spectrumPercentage;
    public ColorTransition transition;

    public ColorNode(ColorNode previous, Color color, ColorNode next)
    {
        this.Previous = previous;
        this.Color = color;
        this.SetNext(next);
    }

    public ColorNode(ColorNode previous, Color color)
    {
        this.Previous = previous;
        this.Color = color;
        this.Next = null;
    }

    public ColorNode(Color color, ColorNode next)
    {
        this.Previous = null;
        this.Color = color;
        this.SetNext(next);
    }

    public ColorNode(Color color)
    {
        this.Previous = null;
        this.Color = color;
        this.Next = null;
    }

    public void SetNext(ColorNode next) 
    {
        this.Next= next;
        this.SetTransition();
    }   

    public void SetTransition()
    {
        this.transition = new ColorTransition(
                this.Next.Color.r - this.Color.r,
                this.Next.Color.g - this.Color.g,
                this.Next.Color.b - this.Color.b
        );
    }


}

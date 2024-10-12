using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorNode
{
   
    public Color Color;
    public ColorNode Next;
    public ColorNode Previous;

    public ColorNode(ColorNode previous, Color color, ColorNode next)
    {
        this.Previous = previous;
        this.Color = color;
        this.Next = next;
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
        this.Next = next;
    }

    public ColorNode(Color color)
    {
        this.Previous = null;
        this.Color = color;
        this.Next = null;
    }


}

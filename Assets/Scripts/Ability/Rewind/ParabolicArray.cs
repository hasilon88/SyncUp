using System;
using UnityEngine;

public class ParabolicArray
{

    private Vector2[] array;
    private Vector2 summit;
    private readonly Vector2 point = new Vector2(1f, 0f);
    private float slope;
    private float xIncrement;

    private ParabolicArray(float target, int size)
    {
        this.summit = new Vector2(0f, target);
        this.array = new Vector2[size];
        this.slope = (point.y - summit.y) / MathF.Pow(point.x - summit.x, 2);
        this.xIncrement = point.x / size;
        SetArrayX();
        SetArrayY();
    }

    private void SetArrayX()
    {
        float nX = xIncrement;
        for (int elem = 0; elem < array.Length; elem++)
        {
            array[elem].x = nX;
            nX += xIncrement;
        }
    }

    private void SetArrayY()
    {
        for (int elem = 0; elem < array.Length; elem++)
            array[elem].y = (slope * MathF.Pow(array[elem].x - summit.x, 2)) + summit.y;
    }

    public static Vector2[] GetArray(float target, int size)
    {
        return new ParabolicArray(target, size).array;
    }


}

using System;
using UnityEngine;

//ARRAY TYPE (LOW_FAST_LOW, FAST_LOW_FAST)
public class ParabolicArray
{

    private Vector2[] array;
    private Vector2 summit;
    private int size;
    private readonly Vector2 point = new Vector2(1f, 0f);
    private float slope;
    private float xIncrement;

    private ParabolicArray(float target, int size)
    {
        SetSize(size);
        this.summit = new Vector2(0f, target);
        this.array = new Vector2[size];
        this.slope = (point.y - summit.y) / MathF.Pow(point.x - summit.x, 2);
        this.xIncrement = point.x / (size/2);
        SetVectorsX();
        SetArrayY();
    }

    private void SetSize(int size)
    {
        if (size % 2 != 0) this.size = size - 1; //NOT +1
        else this.size = size;
    }

    private void SetVectorsX()
    {
        int halfSize = size / 2;
        float nX = 0f;
        Vector2[] firstHalf = new Vector2[halfSize];
        Vector2[] secondHalf = new Vector2[halfSize];
        for (int elem = 0; elem < halfSize; elem++)
        {
            firstHalf[elem].x = nX;
            nX += xIncrement;
        }
        Array.Reverse(firstHalf);
        firstHalf.CopyTo(array, 0);
        nX = 0f;
        for (int elem = 0; elem < halfSize; elem++)
        {
            secondHalf[elem].x = nX;
            nX += xIncrement;
        }
        secondHalf.CopyTo(array, halfSize);
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

using UnityEngine;

public static class ArrayUtils
{

    public static float[] AddLast(float[] initialArray, float element)
    {
        float[] temp = new float[initialArray.Length];
        for (int elem = 0; elem < initialArray.Length - 1; elem++)
            temp[elem] = initialArray[elem + 1];
        temp[initialArray.Length - 1] = element;
        return temp;
    }

}

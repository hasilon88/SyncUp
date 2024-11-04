
public class ArrayUtils<T>
{

    public T[] AddLast(T[] initialArray, T element)
    {
        T[] temp = new T[initialArray.Length];
        for (int elem = 0; elem < initialArray.Length - 1; elem++)
            temp[elem] = initialArray[elem + 1];
        temp[initialArray.Length - 1] = element;
        return temp;
    }

}

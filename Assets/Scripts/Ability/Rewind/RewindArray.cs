

/// <summary>
/// new name?
/// </summary>
public class RewindArray<T>
{

    private T[] objects;
    private int index = 0;

    public RewindArray(int size) 
    {
        objects = new T[size];
    }

    private void AddLast(T element)
    {
        T[] temp = new T[objects.Length];
        for (int elem = 0; elem < objects.Length - 1; elem++)
            temp[elem] = objects[elem + 1];
        temp[objects.Length - 1] = element;
        objects = temp;
    }

    public void Add(T element)
    {
        if (index < objects.Length) objects[index++] = element;
        else AddLast(element);
    }

    public T GetLast(int offset = 0)
    {
        if (index - 1 - offset >= 0) return objects[index - 1 - offset];
        return objects[0];
    }

    public T[] GetObjects()
    {
        return objects;
    }

}



/// <summary>
/// new name?
/// </summary>
public class RewindArray<T>
{

    private T[] objects;
    private int index = 0;

    public RewindArray() 
    {
        objects = new T[GlobalStates.Instance.RewindArrayLength];
    }

    public RewindArray<T> Reset()
    {
        index = 0;
        return new RewindArray<T>();
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

    /// <summary>
    /// - gameObject of RewindResponse is set in RewinderScript
    /// - HasToStop will only be set to true if it is returned in PlayerRewinder
    /// </summary>
    public RewindResponse GetLast(int offset = 0)
    {
        if (index - 1 - offset >= 0) return new RewindResponse(objects[index - 1 - offset], false, null);
        return new RewindResponse(objects[0], true, null);
    }

    public T[] GetObjects()
    {
        return objects;
    }

}

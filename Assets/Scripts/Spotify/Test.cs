using UnityEngine;

public class Test : MonoBehaviour
{
    private Controller _controller;

    public Test()
    {
        _controller = new Controller(25);
        TestMethod();
    }

    public async void TestMethod()
    {
        await _controller.Next();
    }
}
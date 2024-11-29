using UnityEngine;
using UnityEngine.UI;


public class UIUtils 
{

    public static void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

}

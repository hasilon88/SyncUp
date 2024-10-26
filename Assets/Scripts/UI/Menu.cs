using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    private delegate void previousInterfaceCallback();

    public Button LevelSelectButton;
    public Button StoreButton;
    public Button ExitButton;

    public Canvas RootCanvas;

    public Canvas LevelSelectorCanvas;

    public Canvas StoreCanvas;

    private void Start()
    {
        this.SetElements();
        LevelSelectButton.onClick.AddListener(LoadLevelSelectorInterface);
        StoreButton.onClick.AddListener(LoadStoreInterface);
        ExitButton.onClick.AddListener(Exit);
        LevelSelectorCanvas.gameObject.SetActive(false);
        StoreCanvas.gameObject.SetActive(false);
        //GlobalStates s = GlobalStates.Instance;
    }

    private void SetElements()
    {
        LevelSelectButton = GameObject.Find("LevelSelectorButton").GetComponent<Button>();
        StoreButton = GameObject.Find("StoreButton").GetComponent<Button>();
        ExitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        LevelSelectorCanvas = GameObject.Find("TestLevelSelector").GetComponent<Canvas>();
        StoreCanvas = GameObject.Find("TestStore").GetComponent<Canvas>();
        RootCanvas = GetComponent<Canvas>();
        RootCanvas.GetComponentsInChildren<Text>();
    }

    public void NavigateMenu(bool rootCanvas, bool storeCanvas, bool levelSelectorCanvas)
    {
        LevelSelectorCanvas.gameObject.SetActive(levelSelectorCanvas);
        StoreCanvas.gameObject.SetActive(storeCanvas);
        RootCanvas.gameObject.SetActive(rootCanvas);
    }

    public void LoadLevelSelectorInterface()
    {
        NavigateMenu(false, false, true);
    }

    public void LoadStoreInterface()
    {
        NavigateMenu(false, true, false);
    }

    public void GoBack()
    {
        NavigateMenu(true, false, false);

    }

    private void Exit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

}

using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MenuUserInterface
{
    MENU,
    STORE,
    LEVEL_SELECTOR
}

public class MenuBehavior : MonoBehaviour
{

    private string completedLevelsPath = "/Scenes/CompletedScenes/Levels/";
    private MenuUserInterface lastInterface = MenuUserInterface.MENU;
    private MenuUserInterface currentInterface = MenuUserInterface.MENU;
    private Button levelSelectorButton;
    private Button storeButton;
    private Button exitButton;
    private Canvas menuCanvas;
    private Canvas levelSelectorCanvas;
    private Canvas storeCanvas;

    //public GameObject LevelSelectButtonPrefab;

    private void Start()
    {
        this.SetElements();
        this.SetButtonEventCallbacks();
        SetLevelSelectorButtons();
        DisableInterface(levelSelectorCanvas);
        DisableInterface(storeCanvas);
    }

    private void DisableInterface(Canvas canvas)
    {
        canvas.gameObject.SetActive(false);
    }

    private void SetElements()
    {
        levelSelectorButton = ComponentUtils.Find<Button>("LevelSelectorButton");
        storeButton = ComponentUtils.Find<Button>("StoreButton");
        exitButton = ComponentUtils.Find<Button>("ExitButton");
        levelSelectorCanvas = ComponentUtils.Find<Canvas>("LevelSelectorOverlay");
        storeCanvas = ComponentUtils.Find<Canvas>("StoreOverlay");
        menuCanvas = ComponentUtils.Find<Canvas>("MenuOverlay");
        this.SetGoBackButtons();
    }

    private void SetLevelSelectorButtons() 
    {
        DirectoryInfo info = new DirectoryInfo(Application.dataPath + completedLevelsPath);
        foreach (FileInfo file in info.GetFiles("*.unity"))
        {
            //levelSelectorCanvas.gameObject.GetComponent<RectTransform>()
            Debug.Log(file.Name.Split(".")[0]);
        }
    }

    private void SetButtonEventCallbacks()
    {
        levelSelectorButton.onClick.AddListener(() => Navigate(MenuUserInterface.LEVEL_SELECTOR));
        storeButton.onClick.AddListener(() => Navigate(MenuUserInterface.STORE));
        exitButton.onClick.AddListener(UIUtils.Exit);
    }

    private void SetGoBackButtons()
    {
        Button[] array = FindObjectsOfType<Button>();
        foreach (Button button in array)
            if (button.name == "GoBackButton")
                button.onClick.AddListener(() => Navigate(lastInterface));
    }

    private void NavigateMenu(bool menuCanvas, bool storeCanvas, bool levelSelectorCanvas, MenuUserInterface nextInterface)
    {
        this.levelSelectorCanvas.gameObject.SetActive(levelSelectorCanvas);
        this.storeCanvas.gameObject.SetActive(storeCanvas);
        this.menuCanvas.gameObject.SetActive(menuCanvas);
        lastInterface = currentInterface;
        currentInterface = nextInterface;
    }

    private void Navigate(MenuUserInterface userInterface)
    {
        switch (userInterface) 
        {
            case MenuUserInterface.STORE:
                NavigateMenu(false, true, false, MenuUserInterface.STORE);
                break;
            case MenuUserInterface.LEVEL_SELECTOR:
                NavigateMenu(false, false, true, MenuUserInterface.LEVEL_SELECTOR);
                break;
            case MenuUserInterface.MENU:
                NavigateMenu(true, false, false, MenuUserInterface.MENU);
                break;
            default:
                NavigateMenu(true, false, false, MenuUserInterface.MENU);
                break;
        }
    }

}

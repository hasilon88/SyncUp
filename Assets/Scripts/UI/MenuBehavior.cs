using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MenuUserInterface
{
    MENU,
    STORE,
    LEVEL_SELECTOR,
    SETTINGS
}

public class MenuBehavior : MonoBehaviour
{

    private string completedLevelsPath = "/Scenes/CompletedScenes/Levels/";
    private MenuUserInterface lastInterface = MenuUserInterface.MENU;
    private MenuUserInterface currentInterface = MenuUserInterface.MENU;
    private Button levelSelectorButton;
    private Button storeButton;
    private Button exitButton;
    private Button settingsButton;
    private Canvas menuCanvas;
    private Canvas levelSelectorCanvas;
    private Canvas storeCanvas;
    private Canvas settingsCanvas;

    public GameObject LevelSelectorButtonPrefab;

    private void Start()
    {
        this.SetElements();
        this.SetButtonEventCallbacks();
        SetLevelSelectorButtons();
        DisableInterface(levelSelectorCanvas);
        DisableInterface(storeCanvas);
        DisableInterface(settingsCanvas);
    }

    private void DisableInterface(Canvas canvas)
    {
        canvas.gameObject.SetActive(false);
    }

    private void SetElements()
    {
        levelSelectorButton = ComponentUtils.Find<Button>("LevelSelectorButton");
        settingsButton = ComponentUtils.Find<Button>("SettingsButton");
        storeButton = ComponentUtils.Find<Button>("StoreButton");
        exitButton = ComponentUtils.Find<Button>("ExitButton");
        levelSelectorCanvas = ComponentUtils.Find<Canvas>("LevelSelectorOverlay");
        storeCanvas = ComponentUtils.Find<Canvas>("StoreOverlay");
        menuCanvas = ComponentUtils.Find<Canvas>("MenuOverlay");
        settingsCanvas = ComponentUtils.Find<Canvas>("SettingsOverlay");
        this.SetGoBackButtons();
    }

    private void SetLevelSelectorButtons() 
    {
        string currentLevelName;
        LevelSelectButtonBehavior behavior;
        int tempHeightOffSet = 30;
        DirectoryInfo info = new DirectoryInfo(Application.dataPath + completedLevelsPath);
        foreach (FileInfo file in info.GetFiles("*.unity"))
        {
            currentLevelName = file.Name.Split(".")[0];
            GameObject button = Instantiate(LevelSelectorButtonPrefab, levelSelectorCanvas.transform);
            behavior = button.GetComponent<LevelSelectButtonBehavior>();
            behavior.LevelName = currentLevelName;
            behavior.SceneName = currentLevelName;
            behavior.Init();
            //levelSelectorCanvas.gameObject.GetComponent<RectTransform>()
        }
    }

    private void SetButtonEventCallbacks()
    {
        levelSelectorButton.onClick.AddListener(() => Navigate(MenuUserInterface.LEVEL_SELECTOR));
        storeButton.onClick.AddListener(() => Navigate(MenuUserInterface.STORE));
        settingsButton.onClick.AddListener(() => Navigate(MenuUserInterface.SETTINGS));
        exitButton.onClick.AddListener(UIUtils.Exit);
    }

    private void SetGoBackButtons()
    {
        Button[] array = FindObjectsOfType<Button>();
        foreach (Button button in array)
            if (button.name == "GoBackButton")
                button.onClick.AddListener(() => Navigate(lastInterface));
    }

    private void NavigateMenu(bool menuCanvas, bool storeCanvas, bool levelSelectorCanvas, bool settingsCanvas,MenuUserInterface nextInterface)
    {
        this.levelSelectorCanvas.gameObject.SetActive(levelSelectorCanvas);
        this.storeCanvas.gameObject.SetActive(storeCanvas);
        this.menuCanvas.gameObject.SetActive(menuCanvas);
        this.settingsCanvas.gameObject.SetActive(settingsCanvas);
        lastInterface = currentInterface;
        currentInterface = nextInterface;
    }

    private void Navigate(MenuUserInterface userInterface)
    {
        switch (userInterface) 
        {
            case MenuUserInterface.STORE:
                NavigateMenu(false, true, false, false, MenuUserInterface.STORE);
                break;
            case MenuUserInterface.LEVEL_SELECTOR:
                NavigateMenu(false, false, true, false, MenuUserInterface.LEVEL_SELECTOR);
                break;
            case MenuUserInterface.MENU:
                NavigateMenu(true, false, false, false, MenuUserInterface.MENU);
                break;
            case MenuUserInterface.SETTINGS:
                NavigateMenu(false, false, false, true, MenuUserInterface.SETTINGS);
                break;
            default:
                NavigateMenu(true, false, false, false, MenuUserInterface.MENU);
                break;
        }
    }

}

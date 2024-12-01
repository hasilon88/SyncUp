using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButtonBehavior : MonoBehaviour
{

    private Button button;
    public string LevelName;
    public string SceneName;

    public void Init()
    {
        button = GetComponent<Button>();
        button.GetComponentInChildren<TextMeshProUGUI>().text = LevelName;
        button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneName);
        });
    }
}

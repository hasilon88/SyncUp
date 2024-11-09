using UnityEngine.UI;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;

public class SpotifyInterfaceBehavior : MonoBehaviour
{

    private Button nextButton;
    private Button previousButton;
    private Button fastForwardButton;
    private Button rewindButton;
    private Button playPauseButton;
    private TextMeshProUGUI playPauseText;
    private SpotifyController spotifyController;

    private async void Start()
    {
        spotifyController = SpotifyController.Instance;
        SetButtons();   
        SetButtonsListenner();
        await ChangePauseButtonState();
    }

    private void SetButtons()
    {
        nextButton = GameObject.Find("NextButton").GetComponent<Button>();
        previousButton = GameObject.Find("PreviousButton").GetComponent<Button>();
        fastForwardButton = GameObject.Find("FastForwardButton").GetComponent<Button>();
        rewindButton = GameObject.Find("RewindButton").GetComponent<Button>();
        playPauseButton = GameObject.Find("PlayPauseButton").GetComponent <Button>();
        playPauseText = playPauseButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void SetButtonsListenner()
    {
        nextButton.onClick.AddListener(async () => await spotifyController.Next());
        previousButton.onClick.AddListener(async () => await spotifyController.Previous());
        fastForwardButton.onClick.AddListener(async () => await spotifyController.FastForward(10));
        rewindButton.onClick.AddListener(async () => await spotifyController.Rewind(10));
        playPauseButton.onClick.AddListener(async () => await HandlePlayPause());
    }

    private async Task HandlePlayPause() //needs an icon
    {
        await spotifyController.TogglePlayPause();
        await ChangePauseButtonState();
    }

    private async Task ChangePauseButtonState()
    {
        if (await spotifyController.GetPlayPauseState()) playPauseText.text = "Pause";
        else playPauseText.text = "Play";
    }

    void Update()
    {
        
    }
}

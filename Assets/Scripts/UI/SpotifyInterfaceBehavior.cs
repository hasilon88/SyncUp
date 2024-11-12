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

    public bool UseKeys = false;
    public KeyCode NextKey;
    public KeyCode PreviousKey;
    public KeyCode TogglePauseKey;

    private async void Start()
    {
        NextKey = KeyCode.RightArrow;
        PreviousKey = KeyCode.LeftArrow;
        TogglePauseKey = KeyCode.DownArrow;
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




    private async void Update()
    {
        if (UseKeys) 
        {
            if (Input.GetKeyDown(NextKey))
            {
                await spotifyController.Next();
            }
            else if (Input.GetKeyUp(PreviousKey))
            {
                await spotifyController.Previous();
            }
            else if (Input.GetKeyDown(TogglePauseKey))
            {
                await HandlePlayPause();
            }
        }
    }
}

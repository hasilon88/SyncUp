using UnityEngine.UI;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class SpotifyInterfaceBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Canvas root;
    private CanvasGroup canvasGroup;
    private Button nextButton;
    private Button previousButton;
    private Button fastForwardButton;
    private Button rewindButton;
    private Button playPauseButton;
    private TextMeshProUGUI playPauseText;
    private SpotifyController spotifyController;

    private Texture pauseImage;
    private Texture playImage;
    private Texture skipImage;
    private Texture previousImage;
    private Texture fastForwardImage;
    private Texture rewindImage;

    private Coroutine brightenCoroutine;
    private Coroutine hideCoroutine;

    //should be in a separate script
    public float FadeAwayWaitSeconds = 1f;
    public float AlphaIncrement = 0.1f;

    public bool UseKeys = false;
    public KeyCode NextKey;
    public KeyCode PreviousKey;
    public KeyCode TogglePauseKey;

    private async void Start()
    {
        root = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        NextKey = KeyCode.RightArrow;
        PreviousKey = KeyCode.LeftArrow;
        TogglePauseKey = KeyCode.DownArrow;
        spotifyController = SpotifyController.Instance;
        SetButtons();
        SetImages();
        SetButtonsListenner();
        await ChangePauseButtonState();
    }

    private Texture GetSpotifyIcon(string name)
    {
        return (Texture)Resources.Load("images/spotify/" + name);
    }

    private void SetImages()
    {
        playImage = GetSpotifyIcon("play-button");
        pauseImage = GetSpotifyIcon("pause.png");
        fastForwardImage = GetSpotifyIcon("fastforward.png");
        rewindImage = GetSpotifyIcon("rewind-button.png");
        skipImage = GetSpotifyIcon("next.png");
        previousImage = GetSpotifyIcon("previous.png");
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        brightenCoroutine = StartCoroutine(Brighten(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Leave");
        if (brightenCoroutine != null) StopCoroutine(brightenCoroutine);
        hideCoroutine = StartCoroutine(Brighten(false));
    }

    private IEnumerator Brighten(bool reverse = true)
    {
        if (reverse)
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += AlphaIncrement;
                yield return new WaitForSeconds(FadeAwayWaitSeconds * Time.deltaTime);
            }
        else
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= AlphaIncrement;
                yield return new WaitForSeconds(FadeAwayWaitSeconds * Time.deltaTime);
            }
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

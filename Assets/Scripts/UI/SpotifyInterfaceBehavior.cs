using UnityEngine.UI;
using UnityEngine;

public class SpotifyInterfaceBehavior : MonoBehaviour
{

    private Button nextButton;
    private Button previousButton;
    private Button fastForwardButton;
    private Button rewindButton;
    private Button playPauseButton;
    private Controller spotifyController;

    private void Start()
    {
        spotifyController = new Controller(26);
        SetButtons();   
        SetButtonsListenner();
    }

    private void SetButtons()
    {
        nextButton = GameObject.Find("NextButton").GetComponent<Button>();
        previousButton = GameObject.Find("PreviousButton").GetComponent<Button>();
        fastForwardButton = GameObject.Find("FastForwardButton").GetComponent<Button>();
        rewindButton = GameObject.Find("RewindButton").GetComponent<Button>();
    }

    private void SetButtonsListenner()
    {
        nextButton.onClick.AddListener(async () => await spotifyController.Next());
        previousButton.onClick.AddListener(async () => await spotifyController.Previous());
        fastForwardButton.onClick.AddListener(async () => await spotifyController.FastForward(10));
        rewindButton.onClick.AddListener(async () => await spotifyController.Rewind(10));
    }

    void Update()
    {
        
    }
}

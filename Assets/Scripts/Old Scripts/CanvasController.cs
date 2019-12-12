using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    [SerializeField] GameObject introCanvas;
    [SerializeField] GameObject startCanvas;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject winCanvas;
    [SerializeField] GameObject loseCanvas;
    [SerializeField] TextMeshProUGUI loseCanvasSystemText;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerState();
    }

    private void HandlePlayerState()
    {
        if (player.State == PlayerStates.Intro)
        {
            introCanvas.SetActive(true);
            startCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            winCanvas.SetActive(false);
            loseCanvas.SetActive(false);
        }
        if (player.State == PlayerStates.Starting || player.State == PlayerStates.Started)
        {
            introCanvas.SetActive(false);
            startCanvas.SetActive(true);
            gameCanvas.SetActive(false);
            winCanvas.SetActive(false);
            loseCanvas.SetActive(false);
        }
        if (player.State == PlayerStates.Playing || player.State == PlayerStates.Inprogress)
        {
            introCanvas.SetActive(false);
            startCanvas.SetActive(false);
            gameCanvas.SetActive(true);
            winCanvas.SetActive(false);
            loseCanvas.SetActive(false);
        }
        else if (player.State == PlayerStates.Winning)
        {
            introCanvas.SetActive(false);
            startCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            winCanvas.SetActive(true);
            loseCanvas.SetActive(false);
        }
        else if (player.State == PlayerStates.Losing)
        {
            introCanvas.SetActive(false);
            startCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            winCanvas.SetActive(false);
            loseCanvas.SetActive(true);
        }
    }

    public void LostState(string reason)
    {
        player.State = PlayerStates.Losing;
        if (reason == "grades")
        {
            loseCanvasSystemText.text = "You've failed the semester!";
        }
        else if (reason == "team")
        {
            loseCanvasSystemText.text = "You've been kicked off the team!";
        }
        else if (reason == "both")
        {
            loseCanvasSystemText.text = "You've failed the semester AND were kicked off the team!";
        }
    }
}

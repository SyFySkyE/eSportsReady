using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] GameObject startCanvas;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject winCanvas;
    [SerializeField] GameObject loseCanvas;

    [SerializeField] Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerState();
    }

    private void HandlePlayerState()
    {
        if (player.State == PlayerStates.Starting || player.State == PlayerStates.Started)
        {
            startCanvas.SetActive(true);
            gameCanvas.SetActive(false);
            winCanvas.SetActive(false);
            loseCanvas.SetActive(false);
        }
        if (player.State == PlayerStates.Playing || player.State == PlayerStates.Inprogress)
        {
            startCanvas.SetActive(false);
            gameCanvas.SetActive(true);
            winCanvas.SetActive(false);
            loseCanvas.SetActive(false);
        }
        else if (player.State == PlayerStates.Winning)
        {
            startCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            winCanvas.SetActive(true);
            loseCanvas.SetActive(false);
        }
        else if (player.State == PlayerStates.Losing)
        {
            startCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            winCanvas.SetActive(false);
            loseCanvas.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] private Canvas startCanvas;
    [SerializeField] private Canvas inProgressCanvas;
    [SerializeField] private Canvas WinCanvas;
    [SerializeField] private Canvas loseCanvas;

    [SerializeField] private TextMeshProUGUI lossText;

    public enum GameStates { Starting, Started, InProgress, Progressed, Wining, Win, Losing, Loss }
    private GameStates state;
    public GameStates State
    {
        get { return state; }
        set
        {
            if (state != value)
            {
                state = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        State = GameStates.Starting;
    }

    // Update is called once per frame
    void Update()
    {
        HandleStates();
    }

    private void HandleStates()
    {
        switch (State)
        {
            case GameStates.Starting:
                startCanvas.enabled = true;
                inProgressCanvas.enabled = false;
                WinCanvas.enabled = false;
                loseCanvas.enabled = false;
                State = GameStates.Started;
                break;
            case GameStates.InProgress:
                startCanvas.enabled = false;
                inProgressCanvas.enabled = true;
                WinCanvas.enabled = false;
                loseCanvas.enabled = false;
                State = GameStates.Progressed;
                break;
            case GameStates.Wining:
                startCanvas.enabled = false;
                inProgressCanvas.enabled = false;
                WinCanvas.enabled = true;
                loseCanvas.enabled = false;
                State = GameStates.Win;
                break;
            case GameStates.Losing:
                startCanvas.enabled = false;
                inProgressCanvas.enabled = false;
                WinCanvas.enabled = false;
                loseCanvas.enabled = true;
                State = GameStates.Loss;
                break;
        }
    }

    public void LossTrigger(string reason)
    {
        switch (reason)
        {
            case "grades":
                lossText.text = "You've failed the semester!";
                break;
            case "team":
                lossText.text = "You've been kicked off the eSports team!";
                break;
        }
    }
}

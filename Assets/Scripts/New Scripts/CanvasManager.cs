using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [Header("Canvas Objects")]
    [SerializeField] GameObject startCanvas;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject winCanvas;
    [SerializeField] GameObject loseCanvas;

    [Header("TMP Pool Text Objects")]
    [SerializeField] TextMeshProUGUI startText;    
    [SerializeField] TextMeshProUGUI energyPool;
    [SerializeField] TextMeshProUGUI leagueRankPool;
    [SerializeField] TextMeshProUGUI gradePool;
    [SerializeField] TextMeshProUGUI socialStatusPool;

    [Header("TMP System Message Components")]
    [SerializeField] TextMeshProUGUI startMessageText;
    [SerializeField] TextMeshProUGUI gameMessageText;
    [SerializeField] TextMeshProUGUI loseCanvasSystemText;

    public void EnableStartCanvas()
    {
        startCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
    }

    public void EnableGameCanvas()
    {
        startCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
    }

    public void EnableWinCanvas()
    {
        startCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        winCanvas.SetActive(true);
        loseCanvas.SetActive(false);
    }

    public void EnableLoseCanvas(string lossReason)
    {
        startCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        winCanvas.SetActive(false);
        loseCanvas.SetActive(true);

        loseCanvasSystemText.text = lossReason;
    }
}

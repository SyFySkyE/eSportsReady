using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //added scene manager
using TMPro;

public class HUDObserver : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats; // The Observer will need to depend on the observable event, but the script caling the event does not need to know about who receives it and what they do with it. 
    [SerializeField] private DayProgression dayProgression;
    [SerializeField] private ButtonBehaviors ui;

    [Header("TMP Text Objects")]
    [SerializeField] private TextMeshProUGUI dayNumberText;
    [SerializeField] private TextMeshProUGUI gpaProjectionText;
    [SerializeField] private TextMeshProUGUI leagueRankText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI hoursLeftInDayText;
    [SerializeField] private TextMeshProUGUI stressStatusText;
    [SerializeField] private TextMeshProUGUI studyGate;
    [SerializeField] private TextMeshProUGUI practiceGate;
    [SerializeField] private TextMeshProUGUI crunchValue;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI lostMessageText;

    [Header("Canvas objects")]
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject creditsCanvas; // added creditsCanvas

    private void OnEnable() // Should be onEnable if the event firing is in start, otherwise the event may fire before subscribers get a chance to subscribe. Firing events w/out subscribers causes a NullReferenceException. Can be avoided using null checks.
    {
        ui.OnStartPress += Ui_OnStartPress;
        //start of new code
        ui.OnCreditsPress += Ui_OnCreditsPress;
        ui.OnBTStartPress += Ui_OnBTStartPress;
        ui.OnEndGamePress += Ui_OnEndGamePress;
        //end of new code
        dayProgression.OnDayIncrement += DayProgression_OnDayIncrement;
        dayProgression.OnHourChange += DayProgression_OnHourDecrement;
        dayProgression.EndOfYear += DayProgression_EndOfYear;
        playerStats.OnStressChange += PlayerStats_OnStressChange; // We subscribe to the event in this way. Tabbing when you += will autocomplete the line and will create the methods below.
        playerStats.OnEnergyChange += PlayerStats_OnEnergyChange;
        playerStats.OnLeagueRankChange += PlayerStats_OnLeagueRankChange;
        playerStats.onGpaProjectionChange += PlayerStats_onGpaProjectionChange;
        playerStats.OnStudyChange += PlayerStats_OnStudyChange;
        playerStats.OnPracticeChange += PlayerStats_OnPracticeChange;
        playerStats.OnCrunchChange += PlayerStats_OnCrunchChange;
        playerStats.OnMessagePush += PlayerStats_OnMessagePush;
        playerStats.OnGameLost += PlayerStats_OnGameLost;
    }

    private void DayProgression_EndOfYear()
    {
        gameCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        startCanvas.SetActive(false);
        winCanvas.SetActive(true);
        creditsCanvas.SetActive(false); // added credits canvas to be set to false 
    }

    //Start of new Code
    private void Ui_OnCreditsPress()
    {
        gameCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        startCanvas.SetActive(false);
        winCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    private void Ui_OnBTStartPress()
    {
        gameCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        startCanvas.SetActive(true);
        winCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }

    private void Ui_OnEndGamePress()
    {
        Application.Quit(1);
    }

    //End of new code

    private void Ui_OnStartPress()
    {
        gameCanvas.SetActive(true);
        loseCanvas.SetActive(false);
        startCanvas.SetActive(false);
    }

    private void PlayerStats_OnGameLost(string obj)
    {
        gameCanvas.SetActive(false);
        loseCanvas.SetActive(true);
        if (obj == "gpa")
        {
            lostMessageText.text = "You've flunked out!";
        }
        else if (obj == "league")
        {
            lostMessageText.text = "You've been kicked off the team!";
        }
    }

    private void PlayerStats_OnMessagePush(string obj)
    {
        messageText.text = obj;
    }

    private void Start()
    {
        messageText.text = "";
        gameCanvas.SetActive(false);
        startCanvas.SetActive(true);
        loseCanvas.SetActive(false);
        winCanvas.SetActive(false);
    }

    private void PlayerStats_OnCrunchChange(bool obj)
    {
        if (obj == true)
        {
            crunchValue.text = "Crunch";
        }
        else
        {
            crunchValue.text = "Normal";
        }
    }

    private void PlayerStats_OnPracticeChange(int obj, int obj2)
    {
        practiceGate.text = $"{obj}/{obj2}";
    }

    private void PlayerStats_OnStudyChange(int obj, int obj2)
    {
        studyGate.text = $"{obj}/{obj2}";
    }

    private void DayProgression_OnHourDecrement()
    {
        hoursLeftInDayText.text = $"{dayProgression.GetHourseLeft()}";
    }

    private void DayProgression_OnDayIncrement()
    {
        dayNumberText.text = $"Day {dayProgression.GetDayNumber()} of 30";
    }

    private void PlayerStats_OnStressChange(string obj)
    {
        stressStatusText.text = $"{obj}";
    }

    private void PlayerStats_OnEnergyChange(int obj)
    {
        energyText.text = obj.ToString();
    }

    private void PlayerStats_OnLeagueRankChange(string obj)
    {
        leagueRankText.text = obj;
    }

    private void PlayerStats_onGpaProjectionChange(float obj)
    {
        gpaProjectionText.text = obj.ToString("F1"); // Shortens to 1 decimal place (0.1)
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //added scene manager
using UnityEngine.UI;
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
    [SerializeField] private TextMeshProUGUI hangoutGate;

    [Header("Canvas objects")]
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private GameObject showWorkCanvas; //added show work canvas

    [Header("Diff Image Objects accompanied by each stat")]
    [Tooltip("Logic for this is within HUDObserver")]
    [SerializeField] private Image gpaProjectionDiff;
    [SerializeField] private Image leagueRankDiff;
    [SerializeField] private Image remainingHoursDiff;
    [SerializeField] private Image energyDiff;
    [SerializeField] private Image stressDiff;
    [SerializeField] private Image studyDiff;
    [SerializeField] private Image practiceDiff;
    [SerializeField] private Image hangoutDiff;

    [Header("Diff Symbols")]
    [SerializeField] private Sprite increaseSymbol;
    [SerializeField] private Sprite decreaseSymbol;
    [SerializeField] private Sprite emptySymbol;
    [SerializeField] private float secBeforeSymbolReset = 3f;

    private void OnEnable() // Should be onEnable if the event firing is in start, otherwise the event may fire before subscribers get a chance to subscribe. Firing events w/out subscribers causes a NullReferenceException. Can be avoided using null checks.
    {        
        ui.OnStartPress += Ui_OnStartPress;
        ui.OnCreditsPress += Ui_OnCreditsPress;
        ui.OnBTStartPress += Ui_OnBTStartPress;
        ui.OnEndGamePress += Ui_OnEndGamePress;
        ui.OnSleepPress += Ui_OnSleepPress;
        ui.OnChillPress += Ui_OnChillPress;
        //start of new code
        ui.OnShowWorkPress += Ui_OnShowWorkPress;
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
        playerStats.OnHangoutChange += PlayerStats_OnHangoutChange;
        playerStats.OnGPALower += PlayerStats_OnGPALower;
        playerStats.OnGPALevelUp += PlayerStats_OnGPALevelUp;
        playerStats.OnLeagueLower += PlayerStats_OnLeagueLower;
        playerStats.OnLeagueRankUp += PlayerStats_OnLeagueRankUp;
        dayProgression.MidtermTime += DayProgression_MidtermTime;
        dayProgression.FinalsTime += DayProgression_FinalsTime;
        dayProgression.TourneyTime += DayProgression_TourneyTime;
    }

    private void PlayerStats_OnLeagueRankUp()
    {
        leagueRankDiff.sprite = increaseSymbol;
        StartCoroutine(FadeDiffImage(leagueRankDiff));
    }

    private void PlayerStats_OnLeagueLower()
    {
        leagueRankDiff.sprite = decreaseSymbol;
        StartCoroutine(FadeDiffImage(leagueRankDiff));
    }

    private void PlayerStats_OnGPALevelUp()
    {
        gpaProjectionDiff.sprite = increaseSymbol;
        StartCoroutine(FadeDiffImage(gpaProjectionDiff));
    }

    private void PlayerStats_OnGPALower()
    {
        gpaProjectionDiff.sprite = decreaseSymbol;
        StartCoroutine(FadeDiffImage(gpaProjectionDiff));
    }

    private void Ui_OnChillPress()
    {
        energyDiff.sprite = increaseSymbol;
        StartCoroutine(FadeDiffImage(energyDiff));
        stressDiff.sprite = decreaseSymbol;
        StartCoroutine(FadeDiffImage(stressDiff));
    }

    private void Ui_OnSleepPress()
    {
        stressDiff.sprite = decreaseSymbol;
        StartCoroutine(FadeDiffImage(stressDiff));
    }

    private void DayProgression_TourneyTime()
    {
        stressDiff.sprite = increaseSymbol;
        StartCoroutine(FadeDiffImage(stressDiff));
    }

    private void DayProgression_FinalsTime()
    {
        stressDiff.sprite = increaseSymbol;
        StartCoroutine(FadeDiffImage(stressDiff));
    }

    private void DayProgression_MidtermTime()
    {
        stressDiff.sprite = increaseSymbol;
        StartCoroutine(FadeDiffImage(stressDiff));
    }

    private void PlayerStats_OnHangoutChange(int arg1, int arg2)
    {
        hangoutGate.text = $"{arg1}/{arg2}";
        if (arg1 < arg2)
        {
            hangoutDiff.sprite = decreaseSymbol;
            stressDiff.sprite = decreaseSymbol;
            StartCoroutine(FadeDiffImage(stressDiff));
        }
        else
        {
            hangoutDiff.sprite = increaseSymbol;
        }
        StartCoroutine(FadeDiffImage(hangoutDiff));
    }

    private IEnumerator FadeDiffImage(Image image)
    {
        yield return new WaitForSeconds(secBeforeSymbolReset);
        image.sprite = emptySymbol;
    }

    private void DayProgression_EndOfYear()
    {
        gameCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        startCanvas.SetActive(false);
        winCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
        showWorkCanvas.SetActive(false);
    }

    
    private void Ui_OnCreditsPress()
    {
        gameCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        startCanvas.SetActive(false);
        winCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
        showWorkCanvas.SetActive(false);
    }

    private void Ui_OnBTStartPress()
    {
        gameCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        startCanvas.SetActive(true);
        winCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        showWorkCanvas.SetActive(false);
    }

    private void Ui_OnEndGamePress()
    {
        Application.Quit(0);
    }

    //Start of new Code
    private void Ui_OnShowWorkPress()
    {
        gameCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        startCanvas.SetActive(false);
        winCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        showWorkCanvas.SetActive(true);
    }

    //End of new code

    private void Ui_OnStartPress()
    {
        gameCanvas.SetActive(true);
        loseCanvas.SetActive(false);
        startCanvas.SetActive(false);
        winCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        showWorkCanvas.SetActive(false);
    }

    private void PlayerStats_OnGameLost(string obj)
    {
        gameCanvas.SetActive(false);
        loseCanvas.SetActive(true);
        startCanvas.SetActive(false);
        winCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        showWorkCanvas.SetActive(false);
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
        loseCanvas.SetActive(false);
        startCanvas.SetActive(true);
        winCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        showWorkCanvas.SetActive(false);
        ResetDiffImages();
    }

    private void PlayerStats_OnCrunchChange(bool obj)
    {
        if (obj == true)
        {
            crunchValue.text = "Crunch";
            stressDiff.sprite = increaseSymbol;
            StartCoroutine(FadeDiffImage(stressDiff));
            energyDiff.sprite = increaseSymbol;
            StartCoroutine(FadeDiffImage(energyDiff));
        }
        else
        {
            crunchValue.text = "Normal";
        }
    }

    private void PlayerStats_OnPracticeChange(int obj, int obj2)
    {        
        if (obj != 0)
        {
            practiceDiff.sprite = increaseSymbol;
            stressDiff.sprite = increaseSymbol;
            StartCoroutine(FadeDiffImage(stressDiff));
            energyDiff.sprite = decreaseSymbol;
            StartCoroutine(FadeDiffImage(energyDiff));
        }
        else if (obj > obj2)
        {
            obj = obj2;
            practiceDiff.sprite = emptySymbol;
        }
        practiceGate.text = $"{obj}/{obj2}";
        StartCoroutine(FadeDiffImage(practiceDiff));
    }

    private void PlayerStats_OnStudyChange(int obj, int obj2)
    {        
        if (obj != 0)
        {
            studyDiff.sprite = increaseSymbol;
            stressDiff.sprite = increaseSymbol;
            StartCoroutine(FadeDiffImage(stressDiff));
            energyDiff.sprite = decreaseSymbol;
            StartCoroutine(FadeDiffImage(energyDiff));
        }
        else if (obj > obj2)
        {
            obj = obj2;            
        }
        studyGate.text = $"{obj}/{obj2}";
        StartCoroutine(FadeDiffImage(studyDiff));
    }

    private void DayProgression_OnHourDecrement() // Should be OnHourChange, not decrement
    {
        hoursLeftInDayText.text = $"{dayProgression.GetHourseLeft()}";
        if (dayProgression.GetHourseLeft() == 6)
        {
            remainingHoursDiff.sprite = increaseSymbol;
        }
        else
        {
            remainingHoursDiff.sprite = decreaseSymbol;
        }
        
        StartCoroutine(FadeDiffImage(remainingHoursDiff));
    }

    private void DayProgression_OnDayIncrement()
    {
        dayNumberText.text = $"Day {dayProgression.GetDayNumber()} of 15";
        remainingHoursDiff.sprite = increaseSymbol;
        energyDiff.sprite = increaseSymbol;
        StartCoroutine(FadeDiffImage(remainingHoursDiff));
        StartCoroutine(FadeDiffImage(energyDiff));
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

    private void ResetDiffImages()
    {
        gpaProjectionDiff.sprite = emptySymbol;
        leagueRankDiff.sprite = emptySymbol;
        remainingHoursDiff.sprite = emptySymbol;
        stressDiff.sprite = emptySymbol;
        energyDiff.sprite = emptySymbol;
        stressDiff.sprite = emptySymbol;
        studyDiff.sprite = emptySymbol;
        practiceDiff.sprite = emptySymbol;
        hangoutDiff.sprite = emptySymbol;
    }
}

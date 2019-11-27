using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDObserver : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats; // The Observer will need to depend on the observable event, but the script caling the event does not need to know about who receives it and what they do with it. 
    [SerializeField] private DayProgression dayProgression;

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

    private void OnEnable() // Should be onEnable if the event firing is in start, otherwise the event may fire before subscribers get a chance to subscribe. Firing events w/out subscribers causes a NullReferenceException. Can be avoided using null checks.
    {
        dayProgression.OnDayIncrement += DayProgression_OnDayIncrement;
        dayProgression.OnHourChange += DayProgression_OnHourDecrement;
        playerStats.OnStressChange += PlayerStats_OnStressChange; // We subscribe to the event in this way. Tabbing when you += will autocomplete the line and will create the methods below.
        playerStats.OnEnergyChange += PlayerStats_OnEnergyChange;
        playerStats.OnLeagueRankChange += PlayerStats_OnLeagueRankChange;
        playerStats.onGpaProjectionChange += PlayerStats_onGpaProjectionChange;
        playerStats.OnStudyChange += PlayerStats_OnStudyChange;
        playerStats.OnPracticeChange += PlayerStats_OnPracticeChange;
        playerStats.OnCrunchChange += PlayerStats_OnCrunchChange;
    }

    private void PlayerStats_OnCrunchChange(bool obj)
    {
        if (obj == true)
        {
            crunchValue.text = "Active";
        }
        else
        {
            crunchValue.text = "Not Active";
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

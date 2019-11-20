using System; // In order to use System.Action for observer pattern.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StressLevels { Chillin, Stressed, AHHHHHH }
public enum LeagueRankLevels { Bronze, Silver, Gold, Platinum, Diamond, Master, GrandMaster }

public class PlayerStats : MonoBehaviour
{
    [Header("Player-related Values")]
    [Tooltip("Values at start get set to set of values below")]
    [SerializeField] private int stressValue;
    [SerializeField] private int energyValue;
    [SerializeField] private int leagueRankValue;
    [SerializeField] private float gpaProjection;

    [Header("Increment Amounts")]
    [SerializeField] private int leagueRankIncrementAmount = 25;
    [SerializeField] private float gpaProjectionIncrementAmount = 0.2f;

    [Header("Decrement Amounts")]
    [SerializeField] private int leagueRankDecrementAmount = 25;
    [SerializeField] private float gpaProjectionDecrementAmount = 0.2f;
    [SerializeField] private int chillStressDecrement = 10;
    [SerializeField] private int chillEnergyDecrement = 10;
    [SerializeField] private int hangWithFriendsDecrement = 30;
    [SerializeField] private int fullSleepStressDecrement = 15;
    [SerializeField] private int badSleepStressDecrement = 5;

    [Header("Sleep Values To Set Based On Stress")]
    [SerializeField] private int stressLevel1Sleep = 100;
    [SerializeField] private int stressLevel2Sleep = 70;
    [SerializeField] private int stressLevel3Sleep = 50;

    [Header("Starting Values")]
    [Tooltip("Change these to adjust starting values")]
    [SerializeField] private int startingStressValue = 10;
    [SerializeField] private int startingEnergyValue = 100;
    [SerializeField] private int startingLeagueRankValue = 250;
    [SerializeField] private float startingGpaProjection = 2.5f;

    [Header("Stress Thresholds")]
    [SerializeField] private int maxLevel1Stress = 19;
    [SerializeField] private int maxLevel2Stress = 79;
    [SerializeField] private int maxLevel3Stress = 100;

    [Header("League Rank Thresholds")]
    [SerializeField] private int maxBronze = 99;
    [SerializeField] private int maxSilver = 200;
    [SerializeField] private int maxGold = 300;
    [SerializeField] private int maxPlat = 400;
    [SerializeField] private int maxDiamond = 500;
    [SerializeField] private int maxMaster = 600;
    [SerializeField] private int maxGrandMaster = 700;

    [Header("Player Stats is dependent on Button Behaviors")]
    [SerializeField] private ButtonBehaviors ui;

    [Header("Depends on Day Progression Object")]
    [SerializeField] private DayProgression dayProgression;

    private bool canHangWithFriends;
    public event Action<string> OnStressChange;
    public event Action<int> OnEnergyChange;
    public event Action<string> OnLeagueRankChange;
    public event Action<float> onGpaProjectionChange;

    private StressLevels currentStressLevel;
    private LeagueRankLevels currentLeagueRank;

    // Start is called before the first frame update
    void Start()
    {
        StartObservingButtonBehavior();
        InitializeStartingValues();
    }

    private void StartObservingButtonBehavior()
    {
        ui.OnSleepPress += Ui_OnSleepPress; // Subscribing to button events
        ui.OnStudyPress += Ui_OnStudyPress;
        ui.OnPracticePress += Ui_OnPracticePress;
        ui.OnChillPress += Ui_OnChillPress;
        ui.OnHangOutPress += Ui_OnHangOutPress;
    }

    private void OnDisable()
    {
         // Here you would unsubscribe using -= instead of +=. Observing is pretty efficient, but if you ever need to subscribe or open some kind of data stream, you should also think about closing it.
    }

    private void Ui_OnHangOutPress()
    {
        stressValue -= hangWithFriendsDecrement;
        OnStressChange(currentStressLevel.ToString());
    }

    private void Ui_OnChillPress()
    {
        
    }

    private void Ui_OnPracticePress()
    {
        
    }

    private void Ui_OnStudyPress()
    {
        
    }

    private void Ui_OnSleepPress()
    {
        
    }

    private void InitializeStartingValues()
    {
        canHangWithFriends = true;
        currentStressLevel = StressLevels.Chillin;
        stressValue = startingStressValue;
        energyValue = startingEnergyValue;
        leagueRankValue = startingLeagueRankValue;
        gpaProjection = startingGpaProjection;
        currentLeagueRank = LeagueRankLevels.Gold;

        OnStressChange(currentStressLevel.ToString());
        OnEnergyChange(energyValue);
        OnLeagueRankChange(currentLeagueRank.ToString());
        onGpaProjectionChange(gpaProjection);
    }

    private void StressChange()
    {
        if (stressValue >= 0 && stressValue < maxLevel1Stress)
        {
            currentStressLevel = StressLevels.Chillin;
        }
        else if (stressValue > maxLevel1Stress && startingEnergyValue < maxLevel2Stress)
        {
            currentStressLevel = StressLevels.Stressed;
        }
        else
        {
            currentStressLevel = StressLevels.AHHHHHH;
        }
    }

    private void LeagueRankManage()
    {
        if (leagueRankValue > 0 && leagueRankValue < maxBronze)
        {
            currentLeagueRank = LeagueRankLevels.Bronze;
        }
        else if (leagueRankValue > maxBronze && leagueRankValue < maxSilver)
        {
            currentLeagueRank = LeagueRankLevels.Silver;
        }
        if (leagueRankValue > maxSilver && leagueRankValue < maxGold)
        {
            currentLeagueRank = LeagueRankLevels.Gold;
        }
        if (leagueRankValue > maxGold && leagueRankValue < maxPlat)
        {
            currentLeagueRank = LeagueRankLevels.Platinum;
        }
        if (leagueRankValue > maxPlat && leagueRankValue < maxDiamond)
        {
            currentLeagueRank = LeagueRankLevels.Diamond;
        }
        if (leagueRankValue > maxDiamond && leagueRankValue < maxMaster)
        {
            currentLeagueRank = LeagueRankLevels.Master;
        }
        if (leagueRankValue > maxMaster)
        {
            currentLeagueRank = LeagueRankLevels.GrandMaster;
        }
    }
}

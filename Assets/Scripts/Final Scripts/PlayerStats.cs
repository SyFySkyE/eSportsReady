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

    [Header("Gate Values")]    
    [SerializeField] private int currentStudyValue;
    [SerializeField] private int studyGateAmount;
    [SerializeField] private int currentPracticeValue;
    [SerializeField] private int practiceGateAmount;

    [Header("Increment Amounts")]
    [SerializeField] private int leagueRankIncrementAmount = 25;
    [SerializeField] private float gpaProjectionIncrementAmount = 0.2f;
    [SerializeField] private int stressIncrement = 5;
    [SerializeField] private int midtermStressIncrement = 10;
    [SerializeField] private int finalsStressIncrement = 15;
    [SerializeField] private int energyIncrementWhenCrunching = 30;
    [SerializeField] private int stressIncrementWhenCrunching = 30;

    [Header("Decrement Amounts")]
    [SerializeField] private int leagueRankDecrementAmount = 25;
    [SerializeField] private float gpaProjectionDecrementAmount = 0.2f;
    [SerializeField] private int chillStressDecrement = 10;
    [SerializeField] private int standardEnergyDecrement = 10;
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
    [SerializeField] private int startingStudyGateAmount = 2;
    [SerializeField] private int startingPracticeGateAmount = 2;

    [Header("Stress Thresholds")]
    [SerializeField] private int maxLevel1Stress = 19;
    [SerializeField] private int maxLevel2Stress = 79;
    [SerializeField] private int maxLevel3Stress = 100;

    [Header("Max Gate Amounts")]
    [Tooltip("Gate amounts max out at these values, except during midterms and tourneys")]
    [SerializeField] private int maxStudyGate = 6;
    [SerializeField] private int maxPracticeGate = 6;

    [Header("League Rank Thresholds")]
    [SerializeField] private int maxBronze = 99;
    [SerializeField] private int maxSilver = 200;
    [SerializeField] private int maxGold = 300;
    [SerializeField] private int maxPlat = 400;
    [SerializeField] private int maxDiamond = 500;
    [SerializeField] private int maxMaster = 600;
    [SerializeField] private int maxGrandMaster = 700;

    [Header("Lose Conditions")]
    [SerializeField] private float loseGpa = 1f;
    [SerializeField] private int loseLeague = 0;

    [Header("Player Stats is dependent on Button Behaviors")]
    [SerializeField] private ButtonBehaviors ui;

    [Header("Depends on Day Progression Object")]
    [SerializeField] private DayProgression dayProgression;

    private bool canHangWithFriends;
    private bool canChill;
    private bool isCrunchTimeActive;
    private bool wasCrunchTimeActiveYesterday;

    public event Action<string> OnStressChange;
    public event Action<int> OnEnergyChange;
    public event Action<string> OnLeagueRankChange;
    public event Action<float> onGpaProjectionChange;

    public event Action<int, int> OnStudyChange; // What player value is at, what gate value is at
    public event Action<int, int> OnPracticeChange;
    public event Action<bool> OnCrunchChange;
    public event Action<string> OnMessagePush;
    public event Action<string> OnGameLost;

    public event Action OnGPALevelUp;
    public event Action OnLeagueRankUp;
    public event Action OnGPALower;
    public event Action OnLeagueLower;
    public event Action OnStresedOut;
    public event Action OnSuperStressedOut;

    private StressLevels currentStressLevel;
    private LeagueRankLevels currentLeagueRank;

    // Start is called before the first frame update
    void Start()
    {        
        StartObservingButtonBehavior();
        StartObservingDayBehavior();
        InitializeStartingValues();
    }

    private void StartObservingButtonBehavior()
    {
        ui.OnSleepPress += Ui_OnSleepPress; // Subscribing to button events
        ui.OnStudyPress += Ui_OnStudyPress;
        ui.OnPracticePress += Ui_OnPracticePress;
        ui.OnChillPress += Ui_OnChillPress;
        ui.OnHangOutPress += Ui_OnHangOutPress;
        ui.OnCrunchPress += Ui_OnCrunchPress;
    }

    private void Ui_OnCrunchPress()
    {
        if (!wasCrunchTimeActiveYesterday && !isCrunchTimeActive)
        {
            OnMessagePush("You are crunching. Watch your stress!");
            canChill = false;
            canHangWithFriends = false;
            isCrunchTimeActive = true;
            OnCrunchChange(isCrunchTimeActive);
            dayProgression.CrunchHours();
            energyValue += energyIncrementWhenCrunching;
            PushEnergyChange();
            stressValue += stressIncrementWhenCrunching;
            StressChange();
        }        
    }

    private void StartObservingDayBehavior()
    {
        dayProgression.OnDayIncrement += DayProgression_OnDayIncrement;
        dayProgression.TourneyTime += DayProgression_TourneyTime;
        dayProgression.PostTourney += DayProgression_PostTourney;
        dayProgression.MidtermTime += DayProgression_MidtermTime;
        dayProgression.PostMidterm += DayProgression_PostMidterm;
        dayProgression.FinalsTime += DayProgression_FinalsTime;
        dayProgression.PostFinal += DayProgression_PostFinal;
    }

    private void DayProgression_PostFinal()
    {
        studyGateAmount--;
        studyGateAmount--;
        OnStudyChange(currentStudyValue, studyGateAmount);
    }

    private void DayProgression_PostMidterm()
    {
        studyGateAmount--;
        OnStudyChange(currentStudyValue, studyGateAmount);
    }

    private void DayProgression_PostTourney()
    {
        practiceGateAmount--;
        practiceGateAmount--;
        OnPracticeChange(currentPracticeValue, practiceGateAmount);
    }

    private void DayProgression_FinalsTime()
    {
        OnMessagePush("Finals Time! Last chance to up your grades.");
        stressValue += finalsStressIncrement;
        studyGateAmount++;
        studyGateAmount++;
        OnStudyChange(currentStudyValue, studyGateAmount);
    }

    private void DayProgression_MidtermTime()
    {
        OnMessagePush("Time for midterms. Study up!");
        stressValue += midtermStressIncrement;
        studyGateAmount++;
        OnStudyChange(currentStudyValue, studyGateAmount);
    }

    private void DayProgression_TourneyTime()
    {
        OnMessagePush("Tournament Time. Your team needs you to do your best!");
        practiceGateAmount++;
        practiceGateAmount++;
        OnPracticeChange(currentPracticeValue, practiceGateAmount);
    }

    private void DayProgression_OnDayIncrement()
    {
        if (isCrunchTimeActive)
        {
            wasCrunchTimeActiveYesterday = true;
            isCrunchTimeActive = false;
            canChill = true;
            OnCrunchChange(isCrunchTimeActive);
            dayProgression.ResetHours();
        }
        else
        {
            wasCrunchTimeActiveYesterday = false;
        }
        canHangWithFriends = true;
        DailyGPAEvaluation();
        DailyPracticeEvaluation();
    }

    private void DailyGPAEvaluation()
    {
        if (gpaProjection <= loseGpa)
        {
            OnGameLost("gpa");
        }
        else if (leagueRankValue <= loseLeague) // TODO Why is this here??
        {
            OnGameLost("league");
        }
        if (currentStudyValue >= studyGateAmount)
        {
            gpaProjection += gpaProjectionIncrementAmount;
            if (studyGateAmount < maxStudyGate)
            {
                studyGateAmount++;
                OnGPALevelUp();
            }
        }
        else if (currentStudyValue < (studyGateAmount / 2))
        {
            gpaProjection -= gpaProjectionDecrementAmount;
            stressValue += stressIncrement;
            OnGPALower();
        }
        currentStudyValue = 0;
        onGpaProjectionChange(gpaProjection);
        OnStudyChange(currentStudyValue, studyGateAmount);
        StressChange();
    }

    private void DailyPracticeEvaluation()
    {
        if (currentPracticeValue >= practiceGateAmount)
        {
            leagueRankValue += leagueRankIncrementAmount;
            if (practiceGateAmount < maxPracticeGate)
            {
                practiceGateAmount++;
                OnLeagueRankUp();
            }
        }
        else if (currentPracticeValue < (practiceGateAmount / 2))
        {
            leagueRankValue -= leagueRankDecrementAmount;
            OnLeagueLower();
            stressValue += stressIncrement;
        }
        currentPracticeValue = 0;
        onGpaProjectionChange(gpaProjection);
        LeagueRankManage();
        OnLeagueRankChange(currentLeagueRank.ToString());
        OnPracticeChange(currentPracticeValue, practiceGateAmount);
        StressChange();
    }

    private void Ui_OnHangOutPress()
    {        
        if (canHangWithFriends)
        {
            if (dayProgression.GetHourseLeft() <= 0)
            {
                stressValue += stressIncrement;
                OnMessagePush("You're VERY tired");
            }
            OnMessagePush("You hung out with your friends!");
            stressValue -= hangWithFriendsDecrement;
            //start of new code
            energyValue -= standardEnergyDecrement;
            //end of new code
            StressChange();            
        }
        else
        {
            OnMessagePush("You already have hung out with your friends. Try tomorrow!");
        }
        if (isCrunchTimeActive)
        {
            OnMessagePush("You're too busy crunching!");
        }
        canHangWithFriends = false;
    }

    private void Ui_OnChillPress()
    {        
        if (canChill)
        {
            OnMessagePush("You spent some time relaxing.");
            if (dayProgression.GetHourseLeft() <= 0)
            {
                stressValue += stressIncrement;
                OnMessagePush("You're VERY tired");
            }
            dayProgression.DecrementHour();
            stressValue -= chillStressDecrement;
            // Changed line: energyValue -= standardEnergyDecrement; to following:
            energyValue += 20;
            //end of changes
            StressChange();
            PushEnergyChange();
        }
        else
        {
            OnMessagePush("You're too busy crunching!");
        }
    }

    private void Ui_OnPracticePress()
    {
       //Added "if practiceGameAmount is less than the max, execute as normal, else if pGA is more than or equal to practice gate max, do not inrease pGA, do not increase stress, do not increment time, send message "You have studied all you can today"
     if (currentPracticeValue < practiceGateAmount)
        {
            if (dayProgression.GetHourseLeft() <= 0)
            {
                stressValue += stressIncrement;
                OnMessagePush("You're VERY tired!");
            }

            switch (currentStressLevel)
            {
                case StressLevels.Chillin:
                    currentPracticeValue++;
                    currentPracticeValue++;
                    break;
                case StressLevels.Stressed:
                    currentPracticeValue++;
                    break;
                case StressLevels.AHHHHHH:
                    break;
            }
            stressValue += stressIncrement;
            energyValue -= standardEnergyDecrement;            
            StressChange();
            OnPracticeChange(currentPracticeValue, practiceGateAmount);
            dayProgression.DecrementHour();
            PushEnergyChange();
        }
        //Start of new code
        if (currentPracticeValue >= practiceGateAmount)
        {
            currentPracticeValue = practiceGateAmount;
            OnPracticeChange(currentPracticeValue, practiceGateAmount);
            OnMessagePush("You've done all the practice you can today");
        }
        //end of new code
    }

    private void PushEnergyChange()
    {        
        if (energyValue <= 0)
        {
            Ui_OnSleepPress();
        }
        OnEnergyChange(energyValue);
    }

    private void Ui_OnStudyPress()

        //Added if statement: if study value is less than study gate, execute as normal, if study value is equal to or greater than study gate amount, set study value to its max and do not increase stress, do not decrease the time, do not decrease energy, send message "youve done all the studying you can do today"
    {
        if (currentStudyValue < studyGateAmount)
        {
            if (dayProgression.GetHourseLeft() <= 0)
            {
                stressValue += stressIncrement;
                OnMessagePush("You're VERY tired");
            }
            switch (currentStressLevel)
            {
                case StressLevels.Chillin:
                    currentStudyValue++;
                    currentStudyValue++;
                    break;
                case StressLevels.Stressed:
                    currentStudyValue++;
                    OnStresedOut();
                    break;
                case StressLevels.AHHHHHH:
                    OnSuperStressedOut();
                    break;
            }
            stressValue += stressIncrement;
            StressChange();
            energyValue -= standardEnergyDecrement;            
            OnStudyChange(currentStudyValue, studyGateAmount);
            dayProgression.DecrementHour();
            PushEnergyChange();

        }
        //start of new code
        if (currentStudyValue >= studyGateAmount)
        {
            currentStudyValue = studyGateAmount;
            OnStudyChange(currentStudyValue, studyGateAmount);
            OnMessagePush("You've done all the studying you can do for today");
        }
        //end of new code
    }

    private void Ui_OnSleepPress()
    {
        switch (currentStressLevel)
        {
            case StressLevels.Chillin:
                energyValue = stressLevel1Sleep;
                stressValue -= fullSleepStressDecrement;
                OnMessagePush("You slept well!");
                break;
            case StressLevels.Stressed:
                energyValue = stressLevel2Sleep;
                stressValue -= badSleepStressDecrement;
                OnMessagePush("You slept okay.");
                break;
            case StressLevels.AHHHHHH:
                energyValue = stressLevel3Sleep;
                OnMessagePush("You slept terribly. Relax a bit!");
                break;
        }
        PushEnergyChange();
        StressChange();        
        dayProgression.IncrementDay();                
    }

    private void InitializeStartingValues()
    {
        canHangWithFriends = true;
        canChill = true;
        wasCrunchTimeActiveYesterday = false;
        currentStressLevel = StressLevels.Chillin;
        stressValue = startingStressValue;
        energyValue = startingEnergyValue;
        leagueRankValue = startingLeagueRankValue;
        gpaProjection = startingGpaProjection;
        currentLeagueRank = LeagueRankLevels.Gold;
        studyGateAmount = startingStudyGateAmount;
        practiceGateAmount = startingPracticeGateAmount;

        OnStressChange(currentStressLevel.ToString());
        OnEnergyChange(energyValue);
        OnLeagueRankChange(currentLeagueRank.ToString());
        onGpaProjectionChange(gpaProjection);
        OnStudyChange(currentStudyValue, studyGateAmount);
        OnPracticeChange(currentPracticeValue, practiceGateAmount);
        OnMessagePush("A new semester awaits... Manage your time wisely!");
    }

    private void StressChange()
    {
        if (stressValue >= 0 && stressValue < maxLevel1Stress)
        {
            currentStressLevel = StressLevels.Chillin;
        }
        else if (stressValue >= maxLevel1Stress && stressValue < maxLevel2Stress)
        {
            currentStressLevel = StressLevels.Stressed;
        }
        else if (stressValue < 0)
        {
            currentStressLevel = StressLevels.Chillin;
            stressValue = 0;
        }
        else if (stressValue > maxLevel3Stress)
        {
            stressValue = maxLevel3Stress;
        }
        else
        {
            currentStressLevel = StressLevels.AHHHHHH;
            OnMessagePush("You're too stressed to do any work. Relax a bit!");
        }
        OnStressChange(currentStressLevel.ToString());
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

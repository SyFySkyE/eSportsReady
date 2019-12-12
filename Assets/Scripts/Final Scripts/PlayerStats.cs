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
    [SerializeField] private int hangoutGate = 1;
    [SerializeField] private int hangoutValue = 1;

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
    [SerializeField] private int chillStressDecrement = 20; //changed from 10 to 20
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
    [SerializeField] private int startingHangoutValue = 1;

    [Header("Stress Thresholds")]
    [SerializeField] private int maxLevel1Stress = 19;
    [SerializeField] private int maxLevel2Stress = 79;
    [SerializeField] private int maxLevel3Stress = 100;

    [Header("Max Gate Amounts")]
    [Tooltip("Gate amounts max out at these values, except during midterms and tourneys")]
    [SerializeField] private int maxStudyGate = 6;
    [SerializeField] private int maxPracticeGate = 6;
    [SerializeField] private int maxHangoutGate = 1;

    [Header("League Rank Thresholds")]
    [SerializeField] private int maxBronze = 99;
    [SerializeField] private int maxSilver = 200;
    [SerializeField] private int maxGold = 300;
    [SerializeField] private int maxPlat = 400;
    [SerializeField] private int maxDiamond = 500;
    [SerializeField] private int maxMaster = 600;
    [SerializeField] private int maxGrandMaster = 700;

    [Header("Rank Sprites")]
    [SerializeField] SpriteRenderer rankSprite;
    [SerializeField] Sprite bronzeSprite;
    [SerializeField] Sprite silverSprite;
    [SerializeField] Sprite goldSprite;
    [SerializeField] Sprite platinumSprite;
    [SerializeField] Sprite diamondSprite;
    [SerializeField] Sprite mastersSprite;

    [Header("Lose Conditions")]
    [SerializeField] private float loseGpa = 2f;
    [SerializeField] private int loseLeague = 90;

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
    public event Action<int, int> OnHangoutChange;

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
        if (!canHangWithFriends)
        {
            hangoutValue++;
            OnHangoutChange(hangoutValue, hangoutGate);
            canHangWithFriends = true;
        }
        DailyGPAEvaluation();
        DailyPracticeEvaluation();
    }

    private void DailyGPAEvaluation()
    {
        if (gpaProjection <= loseGpa)
        {
            OnGameLost("gpa");
        }
        else if (leagueRankValue <= loseLeague) // TODO Why is this in GPA evaluation??
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

        if (dayProgression.GetHourseLeft() <= 0)
        {
            //stressValue += stressIncrement; Removed
            OnMessagePush("You're too tired do anything else today."); //changed from "You are very tired"
        }
        else
        {
            if (canHangWithFriends)
            {
                OnMessagePush("You hung out with your friends! Energy down slightly. Stress significantly down!");
                stressValue -= hangWithFriendsDecrement;
                energyValue -= standardEnergyDecrement;
                hangoutValue--;
                OnHangoutChange(hangoutValue, hangoutGate);
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
    }

    private void Ui_OnChillPress()
    {

        //Start of new code
        if (dayProgression.GetHourseLeft() <= 0)
        {
            OnMessagePush("You're too tired do anything else today.");
        }
        else
        {
            if (energyValue >= 100)
            {
                canChill = false;
                energyValue = 100;
            }
            //end of new code

            if (canChill)
            {
                OnMessagePush("You spent some time relaxing. Energy partially up, stress slightly down.");
                if (dayProgression.GetHourseLeft() <= 0)
                {
                    //stressValue += stressIncrement; removed
                    OnMessagePush("You're too tired do anything else today."); //Changed from "You are very tired"
                }
                dayProgression.DecrementHour();
                stressValue -= chillStressDecrement;
                energyValue += 20;
                StressChange();
                PushEnergyChange();
            }
            else if (isCrunchTimeActive) //changed from else catch to else if crunchtime is active
            {
                OnMessagePush("You're too busy crunching!");
            }
            else //added else catch for maximum amount of energy;
            {
                OnMessagePush("Youre maxed out on relaxin, all chillin, all cool.");
            }
        }
    }

    private void Ui_OnPracticePress()
    {

        if (dayProgression.GetHourseLeft() <= 0)
        {
            //stressValue += stressIncrement; removed
            OnMessagePush("You're too tired do anything else today."); //changed from "you are very stressed"
        }
        else
        {
            if (currentPracticeValue < practiceGateAmount)
            {
                switch (currentStressLevel)
                {
                    case StressLevels.Chillin:
                        currentPracticeValue++;
                        currentPracticeValue++;
                        //Start of new code
                        if (currentPracticeValue > practiceGateAmount)
                        {
                            currentPracticeValue = practiceGateAmount;
                        }
                        //end of new code
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
                if (currentPracticeValue >= practiceGateAmount)
                {
                    OnPracticeChange(currentPracticeValue, practiceGateAmount);
                    currentPracticeValue = practiceGateAmount;

                    OnMessagePush("You've done all the practice you can today");
                }
            }
        
        }
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
        
    {
        if (dayProgression.GetHourseLeft() <= 0)
        {
            //stressValue += stressIncrement; Removed
            OnMessagePush("You're too tired do anything else today."); //Changed from "youre very tired
        }
        else
        {
            if (currentStudyValue < studyGateAmount)
            {
                switch (currentStressLevel)
                {
                    case StressLevels.Chillin:
                        currentStudyValue++;
                        currentStudyValue++;
                        //Start of new code
                        if (currentStudyValue > studyGateAmount)
                        {
                            currentStudyValue = studyGateAmount;
                        }
                        //end of new code
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

            if (currentStudyValue >= studyGateAmount)
            {
                OnStudyChange(currentStudyValue, studyGateAmount);
                currentStudyValue = studyGateAmount;
                OnMessagePush("You've done all the studying you can do for today");
            }
        }
    }

    private void Ui_OnSleepPress()
    {
        switch (currentStressLevel)
        {
            case StressLevels.Chillin:
                energyValue = stressLevel1Sleep;
                stressValue -= fullSleepStressDecrement;
                OnMessagePush("You slept well! Energy refilled.");
                break;
            case StressLevels.Stressed:
                energyValue = stressLevel2Sleep;
                stressValue -= badSleepStressDecrement;
                OnMessagePush("You slept okay. Energy partially refilled.");
                break;
            case StressLevels.AHHHHHH:
                energyValue = stressLevel3Sleep;
                OnMessagePush("You slept terribly. Energy barely refilled. Relax a bit!");
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
            rankSprite.sprite = bronzeSprite;
        }
        else if (leagueRankValue > maxBronze && leagueRankValue < maxSilver)
        {
            currentLeagueRank = LeagueRankLevels.Silver;
            rankSprite.sprite = silverSprite;
        }
        if (leagueRankValue > maxSilver && leagueRankValue < maxGold)
        {
            currentLeagueRank = LeagueRankLevels.Gold;
            rankSprite.sprite = goldSprite;
        }
        if (leagueRankValue > maxGold && leagueRankValue < maxPlat)
        {
            currentLeagueRank = LeagueRankLevels.Platinum;
            rankSprite.sprite = platinumSprite;
        }
        if (leagueRankValue > maxPlat && leagueRankValue < maxDiamond)
        {
            currentLeagueRank = LeagueRankLevels.Diamond;
            rankSprite.sprite = diamondSprite;
        }
        if (leagueRankValue > maxDiamond && leagueRankValue < maxMaster)
        {
            currentLeagueRank = LeagueRankLevels.Master;
            rankSprite.sprite = mastersSprite;
        }
        if (leagueRankValue > maxMaster)
        {
            currentLeagueRank = LeagueRankLevels.GrandMaster;
        }
    }
}

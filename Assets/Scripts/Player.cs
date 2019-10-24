using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PlayerStates { Starting, Started, Playing, Inprogress, Winning, Won, Losing, Lost }

public class Player : MonoBehaviour
{
    [Header("Pool Amounts")]
    [SerializeField] private int energy = 50;
    [SerializeField] private int leagueRank = 3;
    [SerializeField] private int grades = 3;
    [SerializeField] private int socialStatus = 5;

    [Header("Constants")]
    [SerializeField] private const int restEnergy = 50;
    [SerializeField] private const int maxSocialStatus = 10;
    [SerializeField] private const int energyDecrement = 10;

    [Header("Timers")]
    [SerializeField] private int examTimer = 20;
    [SerializeField] private int examLength = 5;
    [SerializeField] private int tourneyTimer = 30;
    [SerializeField] private int tourneyLength = 5;
    [SerializeField] private int winTimer = 10;

    [Header("State Triggers")]
    [SerializeField] private int loseTrigger = 2;
    [SerializeField] private int winTrigger = 7;

    [Header("TMP Text Objects")]
    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] TextMeshProUGUI energyPool;
    [SerializeField] TextMeshProUGUI leagueRankPool;
    [SerializeField] TextMeshProUGUI gradePool;
    [SerializeField] TextMeshProUGUI socialStatusPool;

    [SerializeField] private float secondsBeforeStateChange = 3f;

    private bool gameIsInProgress = false;
    private bool isWinning = false;
    private PlayerStates currentState;
    public PlayerStates State { get { return this.currentState; }
        set
        {
            if (this.currentState != value)
            {
                this.currentState = value;
            }
        }
    }

    private enum WinStates { None, FinalPhaseStart, FinalPhase }
    private WinStates currentWinState = WinStates.None;

    // Start is called before the first frame update
    void Start()
    {        
        currentState = PlayerStates.Starting;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentState);
        switch (currentState)
        {
            case PlayerStates.Starting:                
                StartCoroutine(StartGame());
                State = PlayerStates.Started;
                break;
            case PlayerStates.Playing:
                StartCoroutine(StartStatDecay());
                State = PlayerStates.Inprogress;
                break;
        }        
        LostState();
    }

    private IEnumerator StartGame()
    {
        startText.text = $"Starting semester in {secondsBeforeStateChange}!";
        messageText.text = "";
        yield return new WaitForSeconds(secondsBeforeStateChange);
        State = PlayerStates.Playing;
        gameIsInProgress = true;
    }

    private IEnumerator StartStatDecay()
    {
        DecayStats();
        ExamTime();
        TourneyTime();
        yield return new WaitForSeconds(1f);
        StartCoroutine(StartStatDecay());
    }

    private void DecayStats()
    {
        if (gameIsInProgress)
        {
            grades--;
            leagueRank--;
            socialStatus--;
            UpdateTextFields();
        }        
    }

    private void ExamTime()
    {        
        if (examTimer <= 0)
        {
            if (examLength <= 0)
            {
                examLength = 5;
                examTimer = 20;
            }
            else
            {
                messageText.text = "Exam Time!";
                examLength--;
                grades--;
            }
        }
        else
        {
            examTimer--;
        }
    }

    private void TourneyTime()
    {
        if (tourneyTimer <= 0)
        {
            if (tourneyLength <= 0)
            {
                tourneyLength = 5;
                tourneyTimer = 30;
            }
            else
            {
                messageText.text = "Tournament Time!";
                tourneyLength--;
                leagueRank--;
            }
        }
        else
        {
            tourneyTimer--;
        }
    }

    private void UpdateTextFields()
    {
        energyPool.text = energy.ToString();
        leagueRankPool.text = leagueRank.ToString();
        gradePool.text = grades.ToString();
        socialStatusPool.text = socialStatus.ToString();
        CheckWin();
    }

    // Button logic. TODO Soc

    public void Sleep()
    {        
        int socialStatusBonus = (socialStatus * 10) / 2;
        energy += restEnergy + socialStatusBonus;
        messageText.text = "You've slept!";
        UpdateTextFields();
    }

    public void Practice()
    {
        if (IsAwake())
        {
            leagueRank++;
            energy -= energyDecrement;
            messageText.text = "You practiced with your team!";
            UpdateTextFields();
        }        
    }

    public void Study()
    {
        if (IsAwake())
        {
            grades++;
            energy -= energyDecrement;
            messageText.text = "You spent some time studying!";
            UpdateTextFields();
        }        
    }

    public void Socialize()
    {
        if (IsAwake())
        {
            if (socialStatus < maxSocialStatus)
            {
                socialStatus++;
                energy -= energyDecrement;
                messageText.text = "You hung out with friends!";
                UpdateTextFields();
            }
        }        
    }

    private bool IsAwake()
    {
        if (energy <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void CheckWin()
    {        
        if (grades >= winTrigger && leagueRank >= winTrigger)
        {
            isWinning = true;
            messageText.text = "You're nearing the top of you class and team!";
            WinChance();
        }
        else
        {
            winTimer = 10; // Reset
            isWinning = false;            
        }
    }

    private void WinChance()
    {
        if (isWinning)
        {            
            winTimer--;  
            if (winTimer == 0)
            {
                currentState = PlayerStates.Winning;
            }
        }        
    }

    private void LostState()
    {
        if (grades <= loseTrigger || leagueRank <= loseTrigger)
        {
            State = PlayerStates.Losing;
        }
    }
}

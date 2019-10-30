using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PlayerStates { Intro, Starting, Started, Playing, Inprogress, Winning, Won, Losing, Lost }
public enum GradeStates { A, B, C, D, F }
public enum LeagueStates { Bronze, Silver, Gold, Platinum, Diamond, Master}

public class Player : MonoBehaviour // TODO Code is baaaaad. Chris is reworking it (scripts in /New Scripts) but prototype will use old code 
{
    [Header("Pool Amounts")]
    [SerializeField] private int energy = 50;
    [SerializeField] private int leagueRank = 1750;
    [SerializeField] private float grades = 4.0f;
    [SerializeField] private int socialStatus = 5;

    [Header("Increment Amounts")]
    [SerializeField] private int restEnergy = 50;
    [SerializeField] private int leagueIncrement = 100;
    [SerializeField] private float gradeIncrement = 0.25f;
    [SerializeField] private int socialStatusIncrement = 1;

    [Header("Decrement Amount")]
    [SerializeField] private int energyDecrement = 10;
    [SerializeField] private int leagueDecrement = 50;
    [SerializeField] private float gradeDecrement = 0.1f;
    [SerializeField] private int socialStatusDecrement = 1;

    [Header("Max Pool Amounts")]
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private int maxLeagueRank = 5000;
    [SerializeField] private float maxGrades = 4.0f;
    [SerializeField] private int maxSocialStatus = 10;

    [Header("Timers")]
    [SerializeField] private int examTimer = 20;
    [SerializeField] private int examLength = 5;
    [SerializeField] private int tourneyTimer = 30;
    [SerializeField] private int tourneyLength = 5;
    [SerializeField] private int winTimer = 10;

    [Header("State Triggers")]
    [SerializeField] private float gradeLoseTrigger = 0f;
    [SerializeField] private float gradeWinTrigger = 3.0f;
    [SerializeField] private int leagueWinTrigger = 2500;
    [SerializeField] private int leagueLoseTrigger = 0;

    [Header("TMP Text Objects")]
    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] TextMeshProUGUI energyPool;
    [SerializeField] TextMeshProUGUI leagueRankPool;
    [SerializeField] TextMeshProUGUI gradePool;
    [SerializeField] TextMeshProUGUI socialStatusPool;

    [Header("Grade and League States")]
    [SerializeField] private float aRank = 4.0f;
    [SerializeField] private float bRank = 3.0f;
    [SerializeField] private float cRank = 2.0f;
    [SerializeField] private float dRank = 1.0f;
    [SerializeField] private int masterRank = 3500;
    [SerializeField] private int diamondRank = 3000;
    [SerializeField] private int platinumRank = 2500;
    [SerializeField] private int goldRank = 2000;
    [SerializeField] private int silverRank = 1500;    
    [SerializeField] TextMeshProUGUI leagueCategory;
    [SerializeField] TextMeshProUGUI gradeCategory;

    [Header("Button Sfx")]
    [SerializeField] AudioClip[] buttonSfx;
    [SerializeField] private float buttonVolume = 2f;
    private AudioSource playerAudio;

    private GradeStates currentGrade;
    private LeagueStates currentLeague;

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

    // Start is called before the first frame update
    void Start()
    {        
        playerAudio = GetComponent<AudioSource>();
        PlayRandomButtonSfx();
        currentState = PlayerStates.Intro;
    }

    public void Restart()
    {
        currentState = PlayerStates.Starting;
        // TODO Reset pools to initial values. Game needs to be balanced first.
    }

    // Update is called once per frame
    void Update()
    {
        DisplayPlayerCategories();
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

    private void DisplayPlayerCategories()
    {
        if (grades >= aRank)
        {
            currentGrade = GradeStates.A;
        }
        else if (grades >= bRank && grades < aRank)
        {
            currentGrade = GradeStates.B;
        }
        else if (grades >= cRank && grades < bRank)
        {
            currentGrade = GradeStates.C;
        }
        else if (grades >= dRank && grades < cRank)
        {
            currentGrade = GradeStates.D;
        }
        else
        {
            currentGrade = GradeStates.F;
        }
        gradeCategory.text = currentGrade.ToString();

        if (leagueRank >= masterRank)
        {
            currentLeague = LeagueStates.Master;
        }
        else if (leagueRank >= diamondRank && leagueRank < masterRank)
        {
            currentLeague = LeagueStates.Diamond;
        }
        else if (leagueRank >= platinumRank && leagueRank < diamondRank)
        {
            currentLeague = LeagueStates.Platinum;
        }
        else if (leagueRank >= goldRank && leagueRank < platinumRank)
        {
            currentLeague = LeagueStates.Gold;
        }
        else if (leagueRank >= silverRank && leagueRank < goldRank)
        {
            currentLeague = LeagueStates.Silver;
        }
        else
        {
            currentLeague = LeagueStates.Bronze;
        }
        leagueCategory.text = currentLeague.ToString();
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
        if (gameIsInProgress)
        {
            ExamTime();
            TourneyTime();
        }        
        CheckWin();
        yield return new WaitForSeconds(1f);
        StartCoroutine(StartStatDecay());            
    }

    private void DecayStats()
    {
        if (gameIsInProgress)
        {                     
            grades -= gradeDecrement;
            leagueRank -= leagueDecrement;
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
                grades -= gradeDecrement;
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
                leagueRank -= leagueDecrement;
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
        gradePool.text = grades.ToString("F2");
        socialStatusPool.text = socialStatus.ToString();
    }

    // Button logic. TODO Soc

    public void Sleep()
    {           
        int socialStatusBonus = (socialStatus * 10) / 2;
        energy += restEnergy + socialStatusBonus;
        socialStatus = 0;
        if (energy >= maxEnergy) energy = maxEnergy;
        messageText.text = "You've slept!";
        UpdateTextFields();
        PlayRandomButtonSfx();
    }

    public void Practice()
    {
        if (IsAwake())
        {
            leagueRank += leagueIncrement;
            energy -= energyDecrement;
            if (leagueRank >= maxLeagueRank) leagueRank = maxLeagueRank;
            messageText.text = "You practiced with your team!";
            UpdateTextFields();
            PlayRandomButtonSfx();
        }        
    }

    public void Study()
    {
        if (IsAwake())
        {
            grades += gradeIncrement;
            energy -= energyDecrement;
            if (grades >= maxGrades) grades = maxGrades;
            messageText.text = "You spent some time studying!";
            UpdateTextFields();
            PlayRandomButtonSfx();
        }        
    }

    public void Socialize()
    {
        if (IsAwake())
        {
            if (socialStatus < maxSocialStatus)
            {
                socialStatus += socialStatusIncrement;
                energy -= energyDecrement;
                messageText.text = "You hung out with friends!";
                UpdateTextFields();
                PlayRandomButtonSfx();
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
            messageText.text = "You need to sleep!";
            return true;
        }
    }

    private void CheckWin()
    {        
        if (grades >= gradeWinTrigger && leagueRank >= leagueWinTrigger)
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
        if (grades <= gradeLoseTrigger && leagueRank <= leagueLoseTrigger)
        {
            gameIsInProgress = false;
            GetComponent<CanvasController>().LostState("both");
        }
        else if (grades <= gradeLoseTrigger)
        {
            gameIsInProgress = false;
            GetComponent<CanvasController>().LostState("grades");
        }
        else if (leagueRank <= leagueLoseTrigger)
        {
            gameIsInProgress = false;
            GetComponent<CanvasController>().LostState("team");
        }
    }

    private void PlayRandomButtonSfx()
    {
        int index = Random.Range(0, buttonSfx.Length);
        playerAudio.PlayOneShot(buttonSfx[index], buttonVolume);        
    }
}

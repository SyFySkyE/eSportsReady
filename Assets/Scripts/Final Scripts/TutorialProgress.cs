using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialProgress : MonoBehaviour // TODO The tutorial in this script is extremely hacked together. High tech debt! Fix it!
{
    [Header("Subscribing to events from these objects")]
    [SerializeField] private PlayerStats player;
    [SerializeField] private DayProgression day;
    [SerializeField] private ButtonBehaviors buttons;

    [Header("TMP Object to store text")]
    [SerializeField] private GameObject textContainer; // So we can setActive
    [SerializeField] private TextMeshProUGUI tutorialText;

    [Header("How long to setActive for")]
    [SerializeField] private float secBeforeTextDisable = 6f;

    [Header("Text to display")]
    [SerializeField] private string tournamentTimeText = "The tournament is here. Practice up! We need you!";
    [SerializeField] private string midtermText = "Midterms are here. Make sure to Study!";
    [SerializeField] private string finalsText = "Finals are here. Study up before the year is over!";
    [SerializeField] private string onStressedOut = "You're not gonna work as good stressed out like that. Come chill with us!";
    [SerializeField] private string onSuperStressedOut = "You're way too stressed! Take a day off and chill already!";
    [SerializeField] private string onLeagueRankLower = "Make sure to Practice more! It's showing in scrims.";
    [SerializeField] private string onGpaLower = "One of your teacher says you aren't doing so well. Study more!";
    [SerializeField] private string onGpaLevelUp = "Your teachers say you've been improving academically. Keep it up!";
    [SerializeField] private string onLeagueRankUp = "Your team is really impressed with you. Keep it up!";

    [Header("Tutorial Text")]
    [SerializeField] private string advisorIntro = "I know the team is important, but remember to Study! The further in the semester the more your teachers will expect of you! It's really important to manage your time wisely.";
    [SerializeField] private string coachIntro = "Make sure your league rank is high. The competition is only gonna get harder as the semester goes on. Your team depends on you, rookie! You might need to Crunch if you need more time.";
    [SerializeField] private string friendIntro = "Hey! I know your busy, but make sure to hang out with your friends eveyr so often. It sounds like things are gonna get stressful, so make sure you Chill and Hang out to manage your stress.";

    private int tutorialSection = 0;

    //[Header("Sprites to use")]

    

    private void Awake()
    {
        player.OnGPALevelUp += Player_OnGPALevelUp;
        player.OnGPALower += Player_OnGPALower;
        player.OnLeagueRankUp += Player_OnLeagueRankUp;
        player.OnLeagueLower += Player_OnLeagueLower;
        player.OnStresedOut += Player_OnStresedOut;
        player.OnSuperStressedOut += Player_OnSuperStressedOut;

        day.OnDayIncrement += Day_OnDayIncrement;
        day.MidtermTime += Day_MidtermTime;
        day.FinalsTime += Day_FinalsTime;
        day.TourneyTime += Day_TourneyTime;
        buttons.OnTutTextClose += Buttons_OnTutTextClose;
    }

    private void Day_OnDayIncrement()
    {
        tutorialSection = 10; // TODO Such as haaack. Disabling tutorial if player skips through itr.
    }

    private void Buttons_OnTutTextClose() // Advisor script starts in start method at bottom. TODO This is very bad. Remind Chris to fix it.
    {
        textContainer.SetActive(false);
        if (tutorialSection == 0)
        {
            textContainer.SetActive(true);
            tutorialText.text = coachIntro;
            tutorialSection++;
        }
        else if (tutorialSection == 1)
        {
            textContainer.SetActive(true);
            tutorialText.text = friendIntro;
            tutorialSection++;
        }
    }

    private void Player_OnLeagueRankUp()
    {
        textContainer.SetActive(true);
        tutorialText.text = onLeagueRankUp; 
    }

    private void Day_TourneyTime()
    {
        textContainer.SetActive(true);
        tutorialText.text = tournamentTimeText;
    }

    private void Day_FinalsTime()
    {
        textContainer.SetActive(true);
        tutorialText.text = finalsText;
    }

    private void Day_MidtermTime()
    {
        textContainer.SetActive(true);
        tutorialText.text = midtermText;
    }

    private void Player_OnSuperStressedOut()
    {
        textContainer.SetActive(true);
        tutorialText.text = onSuperStressedOut;
    }

    private void Player_OnStresedOut()
    {
        textContainer.SetActive(true);
        tutorialText.text = onStressedOut;
    }

    private void Player_OnLeagueLower()
    {
        textContainer.SetActive(true);
        tutorialText.text = onLeagueRankLower;
    }

    private void Player_OnGPALower()
    {
        textContainer.SetActive(true);
        tutorialText.text = onGpaLower;
    }

    private void Player_OnGPALevelUp()
    {
        textContainer.SetActive(true);
        tutorialText.text = onGpaLevelUp;
    }

    private void Start()
    {
        textContainer.SetActive(true);
        tutorialText.text = advisorIntro; 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private int energy = 50;
    [SerializeField] private const int restEnergy = 50;
    [SerializeField] private int socialStatus = 5;
    [SerializeField] private const int maxSocialStatus = 10;
    [SerializeField] private int grades = 3;
    [SerializeField] private int leagueRank = 3;
    [SerializeField] private int lossTriger = 2;
    [SerializeField] private int finalPhasetrigger = 7;
    [SerializeField] private int tourneyTimer = 30;
    [SerializeField] private int examTimer = 20;
    [SerializeField] private int tourneyLength = 5;
    [SerializeField] private int examLength = 5;
    [SerializeField] private int energyDecrement = 10;
    [SerializeField] private int finalPhaseCountdown = 10;

    [SerializeField] private TextMeshProUGUI energyPool;
    [SerializeField] private TextMeshProUGUI socialStatusPool;
    [SerializeField] private TextMeshProUGUI gradesPool;
    [SerializeField] private TextMeshProUGUI leagueRankPool;

    [SerializeField] private GameSession gameSession;

    private bool isGameOver = false;
    private bool isTourneyCommencing = false;
    private bool isExamCommencing = false;
    private bool inLastPhase = false;

    private void Start()
    {
        StartCoroutine(DecayStats());
        StartCoroutine(TourneyTime());
        StartCoroutine(ExamTime());
    }

    private IEnumerator DecayStats()
    {
        if (!isGameOver)
        {
            grades--;
            leagueRank--;
            UpdateTextFields();
            yield return new WaitForSeconds(1f);
        }                
    }    

    private IEnumerator TourneyTime()
    {
        if (!isTourneyCommencing)
        {
            tourneyTimer--;
            if (tourneyTimer == 0)
            {
                isTourneyCommencing = true;
            }
            UpdateTextFields();
            yield return new WaitForSeconds(1f);
            tourneyTimer = 30;
        }
        else
        {
            leagueRank--;
            leagueRank--;
            tourneyLength--;
            if (tourneyLength == 0)
            {
                isTourneyCommencing = false;
            }
            UpdateTextFields();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ExamTime()
    {
        if (!isExamCommencing)
        {
            examTimer--;
            if (examTimer == 0)
            {
                isExamCommencing = true;                
            }
            UpdateTextFields();
            yield return new WaitForSeconds(1f);
            examTimer = 20;
        }
        else
        {
            grades--;
            grades--;
            examLength--;
            if (examLength == 0)
            {
                isExamCommencing = false;
            }
            UpdateTextFields();
            yield return new WaitForSeconds(1f);
            examLength = 5;
        }
    }

    private void UpdateTextFields()
    {
        energyPool.text = energy.ToString();
        socialStatusPool.text = socialStatus.ToString();
        gradesPool.text = grades.ToString();
        leagueRankPool.text = leagueRank.ToString();
        if (grades <= lossTriger)
        {
            gameSession.State = GameSession.GameStates.Losing;
            gameSession.LossTrigger("grades");
        }
        if (leagueRank <= lossTriger)
        {
            gameSession.State = GameSession.GameStates.Losing;
            gameSession.LossTrigger("team");
        }
        if (grades >= finalPhasetrigger && leagueRank >= finalPhasetrigger)
        {
            finalPhaseCountdown--;
        }
        else
        {
            finalPhaseCountdown = 10;
        }
        if (finalPhaseCountdown == 0)
        {
            gameSession.State = GameSession.GameStates.Wining;
        }
    }

    public void Rest()
    {
        int socialStatusBonus = (socialStatus * 10) / 2;
        energy += restEnergy + socialStatus;
        UpdateTextFields();
    }

    public void Socialize()
    {
        if (socialStatus < maxSocialStatus)
        {
            socialStatus++;
            energy -= energyDecrement;
        }                
    }

    public void Study()
    {
        grades++;
        energy -= energyDecrement;
        UpdateTextFields();
    }

    public void Practice()
    {
        leagueRank++;
        energy -= energyDecrement;
        UpdateTextFields();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private int energyToAdd = 3;
    [SerializeField] private float gpaToAdd = 0.1f;
    [SerializeField] private int leagueAmountToAdd = 2;

    [SerializeField] private int numberOfDaysPerExam = 5;
    [SerializeField] private int numberOfDaysPerLeagueEvent = 5;
    [SerializeField] private int numberOfDaysSinceLastExam = 0;
    [SerializeField] private int numberOfDaysSinceLastLeagueEvent = 0;

    private bool canPerformAction = true;
    private int dayCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Sleep()
    {
        player.Sleep(energyToAdd);
        dayCounter++;
        numberOfDaysSinceLastExam++;
        numberOfDaysSinceLastLeagueEvent++;
        CheckForEvents();
        CanPlayerPerformAction();
                
    }

    private void CheckForEvents()
    {
        if (numberOfDaysSinceLastExam == numberOfDaysPerExam)
        {
            TakeExam();
        }
        if (numberOfDaysSinceLastLeagueEvent == numberOfDaysPerLeagueEvent)
        {
            PlayTourney();
        }
    }

    private void TakeExam()
    {
        numberOfDaysSinceLastExam = 0;
        player.TakeExam();
    }

    private void PlayTourney()
    {
        numberOfDaysSinceLastLeagueEvent = 0;
        player.PlayTourney();
    }

    public void Study()
    {
        if (canPerformAction)
        {
            player.Study(gpaToAdd);
        }
        CanPlayerPerformAction();
    }

    public void Practice()
    {
        if (canPerformAction)
        {
            player.Practice(leagueAmountToAdd);
        }
        CanPlayerPerformAction();
    }

    private void CanPlayerPerformAction()
    {
        if (player.GetEnergy() == 0)
        {
            canPerformAction = false;
        }
        else
        {
            canPerformAction = true;
        }
    }
}

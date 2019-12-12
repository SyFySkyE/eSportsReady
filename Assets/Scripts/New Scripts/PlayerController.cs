using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Starting, Playing, FinalStretch, Won, Lost }

public class PlayerController : MonoBehaviour
{
    [Header("Pool Amounts")]
    [SerializeField] private int energy = 50;
    [SerializeField] private int leagueRank = 1750;
    [SerializeField] private float grades = 2.5f;
    [SerializeField] private int socialStatus = 5;

    [Header("Decrement Amount")]
    [SerializeField] private int energyDecrement = 10;
    [SerializeField] private float leagueDecrement = 250;
    [SerializeField] private float gradeDecrement = 0.1f;
    [SerializeField] private int socialStatusDecrement = 1;

    [Header("Max Pool Amounts")]
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private int maxLeagueRank = 5000;
    [SerializeField] private float maxGrades = 4.0f;
    [SerializeField] private int maxSocialStatus = 10;

    [Header("Static Amounts")]
    [SerializeField] private int restEnergy = 50;        

    [Header("Timers")]
    [SerializeField] private int examTimer = 20;
    [SerializeField] private int examLength = 5;
    [SerializeField] private int tourneyTimer = 30;
    [SerializeField] private int tourneyLength = 5;
    [SerializeField] private int winTimer = 10;

    [Header("State Triggers")]
    [SerializeField] private int loseTrigger = 2;
    [SerializeField] private int winTrigger = 7;

    private PlayerState currentState;
    private PlayerState previousState;
    public PlayerState CurrentState
    {
        get { return currentState; }
        set
        {
            if (currentState != value)
            {
                PreviousState = currentState;
                currentState = value;
                Debug.Log($"{name}'s state changed from: {PreviousState} to: {currentState}");
                PreviousState = CurrentState;
            }
        }
    }
    public PlayerState PreviousState
    {
        get { return previousState; }
        set
        {
            if (previousState != value)
            {
                Debug.Log($"{name}'s previous state was {PreviousState} and is now {value}");
                previousState = value;                
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = PlayerState.Starting;
        previousState = PlayerState.Starting;
        
    }
}

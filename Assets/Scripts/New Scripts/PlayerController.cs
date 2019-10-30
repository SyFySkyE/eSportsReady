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
        get { return this.currentState; }
        set
        {
            if (this.currentState != value)
            {
                this.PreviousState = this.currentState;
                this.currentState = value;
                Debug.Log($"{this.name}'s state changed from: {this.PreviousState} to: {this.currentState}");
                this.PreviousState = this.CurrentState;
            }
        }
    }
    public PlayerState PreviousState
    {
        get { return this.previousState; }
        set
        {
            if (this.previousState != value)
            {
                Debug.Log($"{this.name}'s previous state was {this.PreviousState} and is now {value}");
                this.previousState = value;                
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.CurrentState = PlayerState.Starting;
        this.previousState = PlayerState.Starting;
        
    }
}

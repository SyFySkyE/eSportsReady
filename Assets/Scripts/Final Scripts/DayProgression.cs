using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayProgression : MonoBehaviour
{
    [Header("Day by Day")]
    [SerializeField] private int currentDayNumber;
    [SerializeField] private int totalDays = 30;

    [Header("Hour by Hour")]
    [SerializeField] private int currentHoursInDay;
    [SerializeField] private int totalHoursInDay = 6;
    [SerializeField] private int totalHoursWhenCrunching = 10;

    [Header("Events")]
    [SerializeField] private int dayOfMidterm = 14;
    [SerializeField] private int dayOfFinals = 30;
    [SerializeField] private int tourneyTime = 7; // Every seven days

    public event Action OnDayIncrement;
    public event Action OnHourChange;
    public event Action MidtermTime;
    public event Action FinalsTime;
    public event Action TourneyTime;

    public event Action PostMidterm;
    public event Action PostFinal;
    public event Action PostTourney;
    public event Action EndOfYear;

    // Start is called before the first frame update
    void Start()
    {
        InitializeValues();
    }

    private void InitializeValues()
    {        
        currentDayNumber = 1;
        currentHoursInDay = totalHoursInDay;
        OnDayIncrement();
        OnHourChange();
    }

    public void DecrementHour()
    {        
        currentHoursInDay--;
        OnHourChange();
    }

    public int GetHourseLeft()
    {
        return this.currentHoursInDay;
    }

    public int GetDayNumber()
    {
        return this.currentDayNumber;
    }

    public void CrunchHours()
    {
        currentHoursInDay = totalHoursWhenCrunching;
        OnHourChange(); 
    }

    public void ResetHours()
    {
        currentHoursInDay = totalHoursInDay;
        OnHourChange();
    }

    public void IncrementDay()
    {
        currentDayNumber++;
        currentHoursInDay = totalHoursInDay;
        OnDayIncrement();
        OnHourChange();
        if (currentDayNumber == dayOfMidterm)
        {
            MidtermTime();
        }
        else if (currentDayNumber == dayOfMidterm + 1)
        {
            PostMidterm();
        }
        else if (currentDayNumber == dayOfFinals)
        {
            FinalsTime();
        }
        else if (currentDayNumber == dayOfFinals + 1)
        {
            PostFinal();
        }
        
        if (currentDayNumber == tourneyTime)
        {
            TourneyTime();            
        }
        else if (currentDayNumber == tourneyTime + 1)
        {
            PostTourney();
            tourneyTime += tourneyTime;
        }
        if (currentDayNumber == totalDays)
        {
            EndOfYear();
        }
    }
}

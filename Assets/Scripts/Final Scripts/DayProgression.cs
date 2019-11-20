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

    public event Action OnDayIncrement;
    public event Action OnHourDecrement;

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
        OnHourDecrement();
    }

    public void DecrementHour()
    {
        OnHourDecrement();
        currentHoursInDay--;        
    }

    public int GetHourseLeft()
    {
        return this.currentHoursInDay;
    }

    public int GetDayNumber()
    {
        return this.currentDayNumber;
    }

    public void IncrementDay()
    {
        currentDayNumber++;
        currentHoursInDay = totalHoursInDay;
        OnDayIncrement();
    }
}

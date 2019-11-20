using System; // For Observer purposes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviors : MonoBehaviour
{
    [Header("UI Buttons")]
    [Tooltip("Buttons send an Event over to Player Stats")]
    [SerializeField] private Button sleepButton;
    [SerializeField] private Button studyButton;
    [SerializeField] private Button chillButton;
    [SerializeField] private Button practiceButton;
    [SerializeField] private Button hangOutButton;

    public event Action OnSleepPress;
    public event Action OnStudyPress;
    public event Action OnChillPress;
    public event Action OnPracticePress;
    public event Action OnHangOutPress;

    // Click Events

    public void Sleep()
    {
        OnSleepPress();
    }

    public void Study()
    {
        OnStudyPress();
    }

    public void Chill()
    {
        OnChillPress();
    }

    public void Practice()
    {
        OnPracticePress();
    }

    public void Hangout()
    {
        OnHangOutPress();
    }
}

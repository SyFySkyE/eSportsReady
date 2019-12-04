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
    [SerializeField] private Button crunchButton;

    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;
    //start of new code
    [SerializeField] private Button backtoStartButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button endButton;
    [SerializeField] private Button clickButton;
    //End of new code
 
    public event Action OnSleepPress;
    public event Action OnStudyPress;
    public event Action OnChillPress;
    public event Action OnPracticePress;
    public event Action OnHangOutPress;
    public event Action OnCrunchPress;
    public event Action OnStartPress;
    public event Action OnResetPress;
    //Start of new code
    public event Action OnCreditsPress;
    public event Action OnBTStartPress;
    public event Action OnEndGamePress;
    public event Action OnClick;
    // Click Events

    public void Click()
    {
        OnClick();
    }

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

    public void Crunch()
    {
        OnCrunchPress();
    }

    public void OnStart()
    {
        OnStartPress();
    }

    public void ResetButton()
    {
        OnResetPress();
    }

    //Start of new code
    public void CreditsButton()
    {
        OnCreditsPress();
    }

    public void BTStartButton()
    {
        OnBTStartPress();
    }

    public void EndGameButton()
    {
        OnEndGamePress();
    }
    //End of new code
}

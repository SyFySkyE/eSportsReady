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
    [SerializeField] private Button backtoStartButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button endButton;
    [SerializeField] private Button closeTutButton;
    //start of new code
    [SerializeField] private Button showWorkButton;
    //end of new code

    private AudioSource audioSource;

    public event Action OnSleepPress;
    public event Action OnStudyPress;
    public event Action OnChillPress;
    public event Action OnPracticePress;
    public event Action OnHangOutPress;
    public event Action OnCrunchPress;
    public event Action OnStartPress;
    public event Action OnResetPress;
    public event Action OnCreditsPress;
    public event Action OnBTStartPress;
    public event Action OnEndGamePress;
    public event Action OnTutTextClose;
    //start of new code
    public event Action OnShowWorkPress;
    // Click Events

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Sleep()
    {
        OnSleepPress();
        audioSource.Play();
    }

    public void Study()
    {
        OnStudyPress();
        audioSource.Play();
    }

    public void Chill()
    {
        OnChillPress();
        audioSource.Play();
    }

    public void Practice()
    {
        OnPracticePress();
        audioSource.Play();
    }

    public void Hangout()
    {
        OnHangOutPress();
        audioSource.Play();
    }

    public void Crunch()
    {
        OnCrunchPress();
        audioSource.Play();
    }

    public void OnStart()
    {
        OnStartPress();
        audioSource.Play();
    }

    public void ResetButton()
    {
        OnResetPress();
        audioSource.Play();
    }
    
    public void CreditsButton()
    {
        OnCreditsPress();
        audioSource.Play();
    }

    public void BTStartButton()
    {
        OnBTStartPress();
        audioSource.Play();
    }

    //start of new code
    public void ShowWorkButton()
    {
        OnShowWorkPress();
    }
    //end of new code

    public void EndGameButton()
    {
        OnEndGamePress();
        audioSource.Play();
    }

    public void CloseTutorial()
    {
        OnTutTextClose();
        audioSource.Play();
    }
}

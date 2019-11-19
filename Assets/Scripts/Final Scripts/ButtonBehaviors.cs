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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

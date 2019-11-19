using System; // In order to use System.Action for observer pattern.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player-related Values")]
    [Tooltip("Starting values get set to second set of values below")]
    [SerializeField] private int stressValue;
    [SerializeField] private int energyValue;
    [SerializeField] private int leagueRankValue;
    [SerializeField] private float gpaProjection;
    
    [Header("Starting Values")]
    [Tooltip("Change these to adjust starting values")]
    [SerializeField] private int startingStressValue = 10;
    [SerializeField] private int startingEnergyValue = 100;
    [SerializeField] private int startingLeagueRankValue = 250;
    [SerializeField] private float startingGpaProjection = 2.5f;

    [Header("Player Stats is dependent on Button Behaviors")]
    [SerializeField] private ButtonBehaviors ui;

    // The Observer pattern is comprised of Events and an Observer. In this case, making these events public and firing them off later, makes these actions observable. Whenever the action gets called, another script that are subscribed to these actions, the observer (in our case, HUDObserver) can respond to the event, connecting them in a way that lets them know about each other in a very controlled way without creating a dependency.

    // So we create some events and make them public. (They remind me a bit of delegates). Also you need to be using System;
    // Notice the <int> and <float>. Think of those like parameters. We can absolutely just write a public event Action OnStressChange and that's totally legal. But in our case, the stat is changing. The action picks up that it's changed, but it doesn't know what about it is changed so we need to tell it to care.
    // Example. Say we go to sleep and stress gets decreased by 10. The stat gets changed so we fire off the event but the HUD doesn't update. Did it not receive the update? It must have because if an event gets fired off and no one is subscribed, we'll actually get an error. (Which is why we sometimes do null checks on whether our event was actually picked up by something else or not). In reality our HUD did pick up the event, but it didn't know what to do with it. It doesn't know that the value was decreased by 10, so we give it that information as well. So in here, when we Sleep, we'll decrease our internal value (this.energyValue -= 10) and we send the event with the number we're decreasing by (once again by 10), the HUDObserver picks up this info, and does the math again (takes the value.
    public event Action<int> OnStressChange;
    public event Action<int> OnEnergyChange;
    public event Action<int> OnLeagueRankChange;
    public event Action<float> onGpaProjectionChange;
    // We then call these Actions whenever we want the observer to notice the change and react however they want.    

    // Start is called before the first frame update
    void Start()
    {
        InitializeStartingValues();
    }

    private void InitializeStartingValues()
    {
        stressValue = startingStressValue;
        energyValue = startingEnergyValue;
        leagueRankValue = startingLeagueRankValue;
        gpaProjection = startingGpaProjection;
    }
}

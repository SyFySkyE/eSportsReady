using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDObserver : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats; // The Observer will need to depend on the observable event, but the script caling the event does not need to know about who receives it and what they do with it. 
    [SerializeField] private int dayNumber;

    // Start is called before the first frame update
    void Start()
    {
        playerStats.OnStressChange += PlayerStats_OnStressChange; // We subscribe to the event in this way. Tabbing when you += will autocomplete the line and will create the methods below.
        playerStats.OnEnergyChange += PlayerStats_OnEnergyChange;
        playerStats.OnLeagueRankChange += PlayerStats_OnLeagueRankChange;
        playerStats.onGpaProjectionChange += PlayerStats_onGpaProjectionChange;
    }  

    private void PlayerStats_OnStressChange(int obj)
    {
        throw new System.NotImplementedException();
    }

    private void PlayerStats_OnEnergyChange(int obj)
    {
        throw new System.NotImplementedException();
    }

    private void PlayerStats_OnLeagueRankChange(int obj)
    {
        throw new System.NotImplementedException();
    }

    private void PlayerStats_onGpaProjectionChange(float obj)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

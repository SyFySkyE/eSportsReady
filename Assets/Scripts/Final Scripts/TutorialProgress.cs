using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialProgress : MonoBehaviour
{
    [Header("Subscribing to events from these objects")]
    [SerializeField] private PlayerStats player;
    [SerializeField] private DayProgression day;

    [Header("TMP Object to store text")]
    [SerializeField] private GameObject textContainer; // So we can setActive
    [SerializeField] private TextMeshProUGUI tutorialText;

    [Header("How long to setActive for")]
    [SerializeField] private float secBeforeTextDisable = 6f;

    //[Header("Sprites to use")]

    private void Awake()
    {
        player.OnGPALevelUp += Player_OnGPALevelUp;
        player.OnGPALower += Player_OnGPALower;
        player.OnLeagueRankChange += Player_OnLeagueRankChange;
        player.OnLeagueLower += Player_OnLeagueLower;
        player.OnStresedOut += Player_OnStresedOut;
        player.OnSuperStressedOut += Player_OnSuperStressedOut;

        day.MidtermTime += Day_MidtermTime;
        day.FinalsTime += Day_FinalsTime;
        day.TourneyTime += Day_TourneyTime;
    }

    private void Day_TourneyTime()
    {
        throw new System.NotImplementedException();
    }

    private void Day_FinalsTime()
    {
        throw new System.NotImplementedException();
    }

    private void Day_MidtermTime()
    {
        throw new System.NotImplementedException();
    }

    private void Player_OnSuperStressedOut()
    {
        throw new System.NotImplementedException();
    }

    private void Player_OnStresedOut()
    {
        throw new System.NotImplementedException();
    }

    private void Player_OnLeagueLower()
    {
        throw new System.NotImplementedException();
    }

    private void Player_OnLeagueRankChange(string obj)
    {
        throw new System.NotImplementedException();
    }

    private void Player_OnGPALower()
    {
        throw new System.NotImplementedException();
    }

    private void Player_OnGPALevelUp()
    {
        throw new System.NotImplementedException();
    }
}

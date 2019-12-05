using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDObserver2 : MonoBehaviour
{
    [SerializeField] private PlayerStats2 playerStats;
    
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private TextMeshProUGUI label2;
    [SerializeField] private TextMeshProUGUI label3;

    private void OnEnable()
    {
        playerStats.OnNumChange += PlayerStats_OnNumChange;
        playerStats.OnNum2Change += PlayerStats_OnNum2Change;
        playerStats.OnNum3Change += PlayerStats_OnNum3Change;
    }

    private void PlayerStats_OnNum3Change(int obj)
    {
        label3.text = obj.ToString();        
    }

    private void PlayerStats_OnNum2Change(int obj)
    {
        label2.text = obj.ToString();
    }

    private void PlayerStats_OnNumChange(int obj)
    {
        label.text = obj.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private int energy = 5;
    [SerializeField] private float gpa = 3.0f;
    [SerializeField] private int teamRank = 1000;
    [SerializeField] private string rankCategory = rank.Bronze.ToString();

    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI gpaText;
    [SerializeField] private TextMeshProUGUI teamRankText;

    private enum rank { Bronze, Silver, Gold, Platinum, Diamond, Master}

    // Start is called before the first frame update
    void Start()
    {
        UpdateTextFields();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateTextFields()
    {
        energyText.text = energy.ToString();
        gpaText.text = gpa.ToString();
        teamRankText.text = teamRank.ToString();
    }
}

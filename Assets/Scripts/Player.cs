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

    [SerializeField] private float gpaDecayMin = 0.1f;
    [SerializeField] private float gpaDecayMax = 0.3f;
    [SerializeField] private int teamRankDecayMin = 1;
    [SerializeField] private int teamRankDecayMax = 4;

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

    public void Sleep(int valueToAdd)
    {
        energy += valueToAdd;
        gpa -= Random.Range(gpaDecayMin, gpaDecayMax);
        teamRank -= Random.Range(teamRankDecayMin, teamRankDecayMax);
        UpdateTextFields();
    }

    public void Study(float valueToAdd)
    {
        gpa += valueToAdd;
        energy--;
        UpdateTextFields();
    }

    public void Practice(int valueToAdd)
    {
        teamRank += valueToAdd;
        energy--;
        UpdateTextFields();
    }

    public int GetEnergy()
    {
        return energy;
    }

    public void TakeExam()
    {

    }

    public void PlayTourney()
    {

    }
}

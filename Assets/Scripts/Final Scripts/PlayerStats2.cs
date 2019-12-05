using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats2 : MonoBehaviour
{
    [SerializeField] private ButtonBehavior2 buttonBehavior2;
    [SerializeField] private int num = 0;
    [SerializeField] private int num2 = 0;
    [SerializeField] private int num3 = 0;

    public event System.Action<int> OnNumChange;
    public event System.Action<int> OnNum2Change;
    public event Action<int> OnNum3Change;


    private void OnEnable()
    {
        buttonBehavior2.OnClick += ButtonBehavior2_OnClick;
        buttonBehavior2.OnClick2 += ButtonBehavior2_OnClick2;
        buttonBehavior2.OnClick3 += ButtonBehavior2_OnClick3;
    }

    private void ButtonBehavior2_OnClick3()
    {
        num3 = 1000;
        OnNum3Change(num3);
    }

    private void ButtonBehavior2_OnClick2()
    {
        num2 = 100;
        OnNum2Change(num2);
    }

    private void ButtonBehavior2_OnClick()
    {
        num = 10;
        OnNumChange(num);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior2 : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;

    public event System.Action OnClick;
    public event System.Action OnClick2;
    public event System.Action OnClick3;

    public void Click()
    {
        OnClick();
    }

    public void Click2()
    {
        OnClick2();
    }

    public void Click3()
    {
        OnClick3();
    }
}

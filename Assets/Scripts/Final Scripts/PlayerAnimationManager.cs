using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private ButtonBehaviors buttons;

    private Animator playerAnim;

    private void OnEnable()
    {
        buttons.OnSleepPress += Buttons_OnSleepPress;
        buttons.OnStudyPress += Buttons_OnStudyPress;
        buttons.OnChillPress += Buttons_OnChillPress;
        buttons.OnPracticePress += Buttons_OnPracticePress;
        buttons.OnHangOutPress += Buttons_OnHangOutPress;
    }

    private void Buttons_OnHangOutPress()
    {
        playerAnim.SetTrigger("HangTrig");
    }

    private void Buttons_OnPracticePress()
    {
        playerAnim.SetTrigger("PracticeTrig");
    }

    private void Buttons_OnChillPress()
    {
        playerAnim.SetTrigger("ChillTrig");
    }

    private void Buttons_OnStudyPress()
    {
        playerAnim.SetTrigger("StudyTrig");
    }

    private void Buttons_OnSleepPress()
    {
        playerAnim.SetTrigger("SleepTrig");
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
    }
}

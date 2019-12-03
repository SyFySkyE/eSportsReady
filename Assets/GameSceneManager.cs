using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private ButtonBehaviors ui;

    private void OnEnable()
    {
        ui.OnResetPress += Ui_OnResetPress;
    }

    public void Ui_OnResetPress()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}

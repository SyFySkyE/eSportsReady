using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private CanvasManager canvasManager;

    // Update is called once per frame
    void Update()
    {
        HandleStates();
    }

    private void HandleStates()
    {
        if (player.CurrentState == PlayerState.Starting)
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        canvasManager.EnableStartCanvas();
        yield return new WaitForSeconds(1f);
    }
}

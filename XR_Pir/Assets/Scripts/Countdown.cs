using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    public TMP_Text countdownText;
    public float timeLeft;
    public Canvas countdownCanvas;

    private bool countdownActive = false;

    void Start()
    {
        countdownCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (GameStateManager.Instance.CurrentState == GameState.CannonballPickedUp && !countdownActive)
        {
            StartCountdown();
        }

        if (GameStateManager.Instance.CurrentState == GameState.ThreeShipsHit)
        {
            countdownActive = false;
        }

        if (timeLeft > 0 && countdownActive)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                timeLeft = 0;
                GameStateManager.Instance.SetState(GameState.LostBattle);
            }

            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void StartCountdown()
    {
        countdownActive = true;
        countdownCanvas.gameObject.SetActive(true);
    }
}
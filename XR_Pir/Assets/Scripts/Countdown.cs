using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{   
    public Canvas canvas;
    
    public TMP_Text countdownText;
    public float timeLeft;
    private bool countdownActive = false;

    public TMP_Text shipsHitText;

    [SerializeField] float loseDialogueLength = 11f, winDialogueLength = 10f;

    void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (GameStateManager.Instance.CurrentState == GameState.CannonballPickedUp && !countdownActive)
        {
            StartCountdown();
        }

        if (GameStateManager.Instance.CurrentState == GameState.OneShipHit)
        {
            shipsHitText.text = "Ships Hit: 1 / 3";
        }

        if(GameStateManager.Instance.CurrentState == GameState.TwoShipsHit)
        {
            shipsHitText.text = "Ships Hit: 2 / 3";
        }

        if (GameStateManager.Instance.CurrentState == GameState.ThreeShipsHit)
        {
            shipsHitText.text = "Ships Hit: 3 / 3";
            countdownActive = false;

            //win so reset scene after dialogue
            Invoke(nameof(ResetScene), winDialogueLength);
        }

        if (timeLeft > 0 && countdownActive)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                timeLeft = 0;
                GameStateManager.Instance.SetState(GameState.LostBattle);

                //lose so reset scene after dialogue
                Invoke(nameof(ResetScene), loseDialogueLength);
            }

            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void StartCountdown()
    {
        canvas.gameObject.SetActive(true);
        countdownActive = true;
    }

    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
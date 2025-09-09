using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public Canvas canvas;

    public TMP_Text countdownText;
    public float timeLeft;
    private bool countdownActive = false;

    public TMP_Text shipsHitText;

    [SerializeField] float loseDialogueLength = 11f, winDialogueLength = 10f;

    [Header("End game UI and restarting the game")]
    [SerializeField] GameObject endGameUIObject;
    [SerializeField] Animator endgameUIAnim;
    [SerializeField] MeshRenderer yButtonMeshRend;
    [SerializeField] GameObject locoTurn, locoMove;
    [SerializeField] GameObject[] shipItems;

    void Start()
    {
        canvas.gameObject.SetActive(false);
        endGameUIObject.SetActive(false);

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

        if (GameStateManager.Instance.CurrentState == GameState.TwoShipsHit)
        {
            shipsHitText.text = "Ships Hit: 2 / 3";
        }

        if (GameStateManager.Instance.CurrentState == GameState.ThreeShipsHit)
        {
            shipsHitText.text = "Ships Hit: 3 / 3";
            countdownActive = false;

            //win so reset scene after dialogue
            //Invoke(nameof(ResetScene), winDialogueLength);

            //show end game ui after dialoge finishes
            Invoke(nameof(EndOfGame), winDialogueLength);
        }

        if (timeLeft > 0 && countdownActive)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                timeLeft = 0;
                GameStateManager.Instance.SetState(GameState.LostBattle);

                //lose so reset scene after dialogue
                //Invoke(nameof(ResetScene), loseDialogueLength);

                //show end game ui after dialoge finishes
                Invoke(nameof(EndOfGame), loseDialogueLength);

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

    void EndOfGame()
    {
        endGameUIObject.SetActive(true);
        endgameUIAnim.Play("EndUIFade");
        yButtonMeshRend.material.color = Color.yellow;

        Invoke(nameof(HideShip), 1.25f);

        locoTurn.SetActive(false);
        locoMove.SetActive(false);

    }

    void HideShip()
    {
        foreach (GameObject go in shipItems)
        {
            foreach (MeshRenderer mr in go.GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = false;
            }
        }
    }
}
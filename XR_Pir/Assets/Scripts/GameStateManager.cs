using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { GameStart, Introduction, TelescopePickedUp, Idle1, Idle2, Idle3, Idle4, ShipSpotted, CannonballPickedUp, OneShipHit, TwoShipsHit, ThreeShipsHit, LostBattle, PreGame }

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    public GameState CurrentState { get; private set; }
    private bool cannonballPickupTriggered = false;

    private float idleTimer = 0f;
    private float idleThreshold = 35f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        SetState(GameState.PreGame);
        //StartCoroutine(SceneLoaded());
    }

    public IEnumerator SceneLoaded()
    {
        yield return new WaitForSeconds(3f);    // allows the scene to load before playing voiceline
        SetState(GameState.GameStart);
    }

    void Update()
    {
        if (CurrentState != GameState.Idle1 && CurrentState != GameState.Idle2 && CurrentState != GameState.Idle3 && CurrentState != GameState.Idle4)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleThreshold)
            {
                TriggerIdleState();
            }
        }
    }

    public void SetState(GameState newState)
    {
        if (newState != GameState.Idle1 && newState != GameState.Idle2 && newState != GameState.Idle3 && newState != GameState.Idle4)
        {
            idleTimer = 0f;     // reset idle timer
        }

        CurrentState = newState;
        DialogueManager.Instance.PlayStateDialogue(newState);
    }

    private void TriggerIdleState()
    {
        if (CurrentState != GameState.Idle1 && CurrentState != GameState.Idle2 && CurrentState != GameState.Idle3 && CurrentState != GameState.Idle4)
        {
            GameState randomIdleState = (GameState)Random.Range((int) GameState.Idle1, (int) GameState.Idle4 + 1);
            SetState(randomIdleState);
        }
    }

    public void WheelTurned()
    {
        SetState(GameState.Introduction);
    }

    public void TelescopePicked()
    {
        SetState(GameState.TelescopePickedUp);
    }

    public void ShipSpotted()
    {
        SetState(GameState.ShipSpotted);
    }

    public void CannonballPickedUp()
    {
        if (!cannonballPickupTriggered)
        {
            cannonballPickupTriggered = true;
            SetState(GameState.CannonballPickedUp);
        }
    }

    public void OneShipHit()
    {
        SetState(GameState.OneShipHit);
    }

    public void TwoShipsHit()
    {
        SetState(GameState.TwoShipsHit);
    }

    public void ThreeShipsHit()
    {
        SetState(GameState.ThreeShipsHit);
    }

    public void LostBattle()
    {
        SetState(GameState.LostBattle);
    }
}
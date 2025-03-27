using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { GameStart, Introduction, TelescopePickedUp, Idle, ShipSpotted, CannonballPickedUp, OneShipHit, TwoShipsHit, ThreeShipsHit, LostBattle }

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
        StartCoroutine(SceneLoaded());
    }

    IEnumerator SceneLoaded()
    {
        yield return new WaitForSeconds(3f);    // allows the scene to load before playing voiceline
        SetState(GameState.GameStart);
    }

    void Update()
    {
        if (CurrentState != GameState.Idle)
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
        if (newState != GameState.Idle)
        {
            idleTimer = 0f;     // reset idle timer
        }

        CurrentState = newState;
        DialogueManager.Instance.PlayStateDialogue(newState);
    }

    private void TriggerIdleState()
    {
        if (CurrentState != GameState.Idle)
        {
            SetState(GameState.Idle);
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
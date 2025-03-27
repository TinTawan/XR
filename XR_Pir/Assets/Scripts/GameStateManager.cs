using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { GameStart, Introduction, TelescopePickedUp, FreePlay, ShipSpotted, CannonballPickedUp, OneShipHit, TwoShipsHit, ThreeShipsHit }

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    public GameState CurrentState { get; private set; }
    bool cannonballPickupTriggered = false;

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

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        DialogueManager.Instance.PlayStateDialogue(newState);
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
}
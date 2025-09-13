using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartUI : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] GameObject startUI;
    [SerializeField] GameObject locoTurn, locoMove, lNearFarInteractor, rNearFarInteractor, lControllerCanvas, rControllerCanvas;
    [SerializeField] Animator menuFadeAnim;

    [Header("Input References for Game Start")]
    [SerializeField] InputActionReference primaryRef; 
    [SerializeField] InputActionReference secondaryRef, lGripRef, rGripRef, lTrigRef, rTrigRef;

    bool gameStarted = false;

    private void OnEnable()
    {
        primaryRef.action.performed += AnyButtonPressed_Performed;
        secondaryRef.action.performed += AnyButtonPressed_Performed;
        lGripRef.action.performed += AnyButtonPressed_Performed;
        rGripRef.action.performed += AnyButtonPressed_Performed;
        lTrigRef.action.performed += AnyButtonPressed_Performed;
        rTrigRef.action.performed += AnyButtonPressed_Performed;
    }

    private void AnyButtonPressed_Performed(InputAction.CallbackContext ctx)
    {    
        
        if (!gameStarted)
        {

            //fade menu out
            menuFadeAnim.speed = 1;

            //allow movement
            locoTurn.SetActive(true);
            locoMove.SetActive(true);

            //allow interaction
            lNearFarInteractor.SetActive(true);
            rNearFarInteractor.SetActive(true);

            //show controller help canvases
            lControllerCanvas.SetActive(true);
            rControllerCanvas.SetActive(true);

            //set game state to playing
            StartCoroutine(GameStateManager.Instance.SceneLoaded());
        }

        gameStarted = true;
        DisableInputRefs();
    }


    private void Start()
    {
        GameStateManager.Instance.SetState(GameState.PreGame);

        locoTurn.SetActive(false);
        locoMove.SetActive(false);

        lNearFarInteractor.SetActive(false);
        rNearFarInteractor.SetActive(false);

        lControllerCanvas.SetActive(false);
        rControllerCanvas.SetActive(false);

        startUI.SetActive(true);

        menuFadeAnim.speed = 0;
    }

    void DisableInputRefs()
    {
        primaryRef.action.performed -= AnyButtonPressed_Performed;
        secondaryRef.action.performed -= AnyButtonPressed_Performed;
        lGripRef.action.performed -= AnyButtonPressed_Performed;
        rGripRef.action.performed -= AnyButtonPressed_Performed;
        lTrigRef.action.performed -= AnyButtonPressed_Performed;
        rTrigRef.action.performed -= AnyButtonPressed_Performed;
    }

    private void OnDisable()
    {
        DisableInputRefs();
    }
}

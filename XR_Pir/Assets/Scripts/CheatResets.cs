using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CheatResets : MonoBehaviour
{
    [SerializeField] private InputActionReference resetActionRef, quitActionRef;

    private void Awake()
    {
        resetActionRef.action.Enable();
        quitActionRef.action.Enable();

        resetActionRef.action.performed += Reset_Performed;
        quitActionRef.action.performed += Quit_Performed;
    }

    private void Quit_Performed(InputAction.CallbackContext ctx)
    {
        Application.Quit();
    }

    private void Reset_Performed(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        resetActionRef.action.Disable();
        quitActionRef.action.Disable();

        resetActionRef.action.performed -= Reset_Performed;
        quitActionRef.action.performed -= Quit_Performed;
    }
}

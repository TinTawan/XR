using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MenuTrigger : MonoBehaviour
{
    public GameObject canvas;
    public Transform wheelTransform;
    public float rotationThreshold = 90f;
    private bool canvasHidden = false;
    private float startRotation;
    public GameObject wheelAffordance;

    public GameObject limitMovementColliders;
    public bool collidersInactive = false;

    public GameObject spyglassObject;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable xrGrabInteractable;

    void Start()
    {
        startRotation = wheelTransform.eulerAngles.y;

        xrGrabInteractable = spyglassObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        xrGrabInteractable.enabled = false;     // disables player from picking up spyglass before turning wheel
    }

    void Update()
    {
        float wheelRotation = wheelTransform.eulerAngles.y - startRotation;

        // Convert to -180 to 180 range for proper comparison
        if (wheelRotation > 180)
        {
            wheelRotation -= 360;
        }
        else if (wheelRotation < -180)
        {
            wheelRotation += 360;
        }

        if (!canvasHidden && !collidersInactive && Mathf.Abs(wheelRotation) >= rotationThreshold)
        {
            canvas.SetActive(false);
            canvasHidden = true;
            GameStateManager.Instance.WheelTurned();
            wheelAffordance.SetActive(false);

            limitMovementColliders.SetActive(false);
            collidersInactive = true;

            xrGrabInteractable.enabled = true;
        }
    }
}
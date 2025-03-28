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

    void Start()
    {
        startRotation = wheelTransform.eulerAngles.y;
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

        if (!canvasHidden && Mathf.Abs(wheelRotation) >= rotationThreshold)
        {
            canvas.SetActive(false);
            canvasHidden = true;
            GameStateManager.Instance.WheelTurned();
        }
    }
}
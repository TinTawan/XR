using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CanvasRotateToFacePlayer : MonoBehaviour
{
    Camera playerCam;
    RectTransform rect;

    private void Start()
    {
        playerCam = Camera.main;
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 look = (rect.transform.position - playerCam.transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(look);

        Vector3 newRot = new(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);
        rect.transform.rotation = Quaternion.Euler(newRot);
    }
}

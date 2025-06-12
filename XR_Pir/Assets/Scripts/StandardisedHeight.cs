using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardisedHeight : MonoBehaviour
{
    [SerializeField] private float defaultHeight = 1.0f;
    [SerializeField] private Transform cameraOffset;
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        StartCoroutine(SetDefaultHeight());
    }

    private IEnumerator SetDefaultHeight()
    {
        yield return new WaitUntil(() => mainCamera.transform.localPosition.y > 0.01f);

        float headsetLocalY = mainCamera.transform.localPosition.y;
        float heightAdjustment = defaultHeight - headsetLocalY;

        Vector3 offsetPosition = cameraOffset.localPosition;
        offsetPosition.y = heightAdjustment;
        cameraOffset.localPosition = offsetPosition;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Telescope : MonoBehaviour
{
    [SerializeField] Transform lHand, rHand;
    bool zoomed;

    [SerializeField] Camera zoomCam;
    Camera mainCam;
    Transform zoomCamOrigin;

    [SerializeField] float zoomLevel = 6f, maxZoomLength = 15f, zoomMultTest = 2f;
    float handDist;


    GameObject telescope;

    private void Start()
    {
        mainCam = Camera.main;
        mainCam.enabled = true;
        zoomCam.enabled = false;

        zoomCamOrigin = zoomCam.transform;
    }

    private void Update()
    {
        if (zoomed && telescope != null)
        {
            ZoomIn();
        }
        else
        {
            mainCam.enabled = true;
            zoomCam.enabled = false;

            zoomCam.transform.position = zoomCamOrigin.position;
        }
    }

    void ZoomIn()
    {
        //handDist = Vector3.Distance(lHand.position, rHand.position);
        float dist = Vector3.Dot(lHand.position - rHand.position, transform.forward);
        handDist = Mathf.Abs(dist) * zoomMultTest;

        Debug.Log(handDist);

        mainCam.enabled = false;
        zoomCam.enabled = true;

        zoomCam.transform.position = new(zoomCam.transform.position.x, zoomCam.transform.position.y, zoomCam.transform.position.z * handDist/* * zoomLevel*/);
        float clampedZ = Mathf.Clamp(zoomCam.transform.position.z, 2f, maxZoomLength);
        zoomCam.transform.position = new(zoomCam.transform.position.x, zoomCam.transform.position.y, clampedZ);
    }


    private void OnTriggerEnter(Collider col)
    {
        Debug.Log($"{col.name} entered trigger");

        if (col.CompareTag("Telescope"))
        {
            zoomed = true;
            telescope = col.gameObject;
        }

    }

    private void OnTriggerExit(Collider col)
    {
        Debug.Log($"{col.name} exited trigger");

        if (col.CompareTag("Telescope"))
        {
            zoomed = false;
            telescope = null;

            zoomCam.transform.position = mainCam.transform.position;
        }
    }
}

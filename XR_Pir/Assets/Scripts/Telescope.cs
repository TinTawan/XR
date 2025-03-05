using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telescope : MonoBehaviour
{
    [SerializeField] MeshRenderer vignette;
    [SerializeField] float vAppertureSizeMax = 0.8f, vAppertureSizeMin = 0.45f, vFeatheringSize = 0.2f;
    float vApperture;

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
        
        vignette.enabled = false;
        vApperture = vAppertureSizeMax;
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
        float dist = Vector3.Dot(lHand.position - rHand.position, transform.forward);
        handDist = Mathf.Abs(dist) * zoomMultTest;

        handDist = Remap(handDist, 0.1f, 1.2f, 0.9f, 1.1f);

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

            StartCoroutine(AppertureZoomIn(0.05f));

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

            StartCoroutine(AppertureZoomOut(0.1f));
        }
    }

    IEnumerator AppertureZoomIn(float delta)
    {
        vignette.enabled = true;

        while (vApperture > vAppertureSizeMin)
        {
            vApperture -= delta;

            vignette.materials[0].SetFloat("_ApertureSize", vApperture);
            yield return null;
        }
    }

    IEnumerator AppertureZoomOut(float delta)
    {
        while (vApperture < vAppertureSizeMax)
        {
            vApperture += delta;

            vignette.materials[0].SetFloat("_ApertureSize", vApperture);
            yield return null;
        }

        vignette.enabled = false;

    }

    float Remap(float value, float a1, float a2, float b1, float b2)
    {
        return b1 + (value - a1) * (b2 - b1) / (a2 - a1);
    }
}

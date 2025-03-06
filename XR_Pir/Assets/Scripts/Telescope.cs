using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class Telescope : MonoBehaviour
{
    [Header("Vignette")]
    [SerializeField] MeshRenderer vignette;
    [SerializeField] float vAppertureSizeMax = 0.8f, vAppertureSizeMin = 0.45f/*, vFeatheringSize = 0.2f*/;
    float vApperture;

    [Header("Hand Transforms")]
    [SerializeField] Transform lHand;
    [SerializeField] Transform rHand;
    bool zoomed;

    [Header("Camera")]
    [SerializeField] Camera zoomCam;
    Camera mainCam;
    Transform zoomCamOrigin;

    [SerializeField] float /*zoomLevel = 6f,*/ maxZoomLength = 15f, zoomMultTest = 2f, zoomSmoothMove = 0.5f;
    float handDist;
    Vector3 velocity = Vector3.zero;


    GameObject telescope;
    XRGrabInteractable telescopeGrab;
    IXRSelectInteractable telescopeInteractable;

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
        if(telescopeInteractable.IsSelectedByLeft() && telescopeInteractable.IsSelectedByRight())
        {
            //held by both hands

            float dist = Vector3.Dot(lHand.position - rHand.position, transform.forward);
            handDist = Mathf.Abs(dist) * zoomMultTest;

            handDist = Remap(handDist, 0.1f, 1.2f, 0.9f, 1.1f);

            mainCam.enabled = false;
            zoomCam.enabled = true;

            //zoom camera forward, zoom length depending on how far the hands are away from eachother
            //zoomCam.transform.rotation = mainCam.transform.rotation;
            float zoomVal = Remap(handDist, 0.9f, 1.1f, 0f, maxZoomLength);
            //zoomCam.transform.position = mainCam.transform.position + (mainCam.transform.forward * zoomVal);

            Vector3 pos = Vector3.SmoothDamp(zoomCam.transform.position, mainCam.transform.position + (mainCam.transform.forward * zoomVal), ref velocity, zoomSmoothMove);

            zoomCam.transform.SetPositionAndRotation(pos, mainCam.transform.rotation);

        }
        else if(telescopeInteractable.IsSelectedByLeft() || telescopeInteractable.IsSelectedByRight())
        {
            //held by one hand

            mainCam.enabled = false;
            zoomCam.enabled = true;

            //zoom camera forward, zoom length depending on how far the hands are away from eachother
            //zoomCam.transform.rotation = mainCam.transform.rotation;
            //float zoomVal = Remap(handDist, 0.9f, 1.1f, 0f, maxZoomLength);
            //zoomCam.transform.position = mainCam.transform.position + (mainCam.transform.forward * zoomVal);

            Vector3 pos = Vector3.SmoothDamp(zoomCam.transform.position, mainCam.transform.position + (mainCam.transform.forward * maxZoomLength / 2), ref velocity, zoomSmoothMove);

            zoomCam.transform.SetPositionAndRotation(pos, mainCam.transform.rotation);
        }
        

    }


    private void OnTriggerEnter(Collider col)
    {
        Debug.Log($"{col.name} entered trigger");

        if (col.CompareTag("Telescope"))
        {
            zoomed = true;
            telescope = col.gameObject;

            telescopeGrab = telescope.GetComponentInParent<XRGrabInteractable>();

            if (telescopeGrab.TryGetComponent(out IXRSelectInteractable select))
            {
                telescopeInteractable = select;
            }
            else
            {
                Debug.LogWarning("Failed to get IXRSelectInteractable");
            }


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
            telescopeGrab = null;

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

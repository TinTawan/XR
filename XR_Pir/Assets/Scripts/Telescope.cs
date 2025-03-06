using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class Telescope : MonoBehaviour
{
    [SerializeField] MeshRenderer vignette;
    [SerializeField] float vAppertureSizeMax = 0.8f, vAppertureSizeMin = 0.45f/*, vFeatheringSize = 0.2f*/;
    float vApperture;

    [SerializeField] Transform lHand, rHand;
    bool zoomed;

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
            zoomCam.transform.rotation = mainCam.transform.rotation;
            float zoomVal = Remap(handDist, 0.9f, 1.1f, 0f, maxZoomLength);
            //zoomCam.transform.position = mainCam.transform.position + (mainCam.transform.forward * zoomVal);

            zoomCam.transform.position = Vector3.SmoothDamp(zoomCam.transform.position, mainCam.transform.position + (mainCam.transform.forward * zoomVal), ref velocity, zoomSmoothMove);
        }
        else if(telescopeInteractable.IsSelectedByLeft() || telescopeInteractable.IsSelectedByRight())
        {
            //held by one hand

            mainCam.enabled = false;
            zoomCam.enabled = true;

            //zoom camera forward, zoom length depending on how far the hands are away from eachother
            zoomCam.transform.rotation = mainCam.transform.rotation;
            //float zoomVal = Remap(handDist, 0.9f, 1.1f, 0f, maxZoomLength);
            //zoomCam.transform.position = mainCam.transform.position + (mainCam.transform.forward * zoomVal);

            zoomCam.transform.position = Vector3.SmoothDamp(zoomCam.transform.position, mainCam.transform.position + (mainCam.transform.forward * maxZoomLength/2), ref velocity, zoomSmoothMove);
        }
        

    }


    private void OnTriggerEnter(Collider col)
    {
        Debug.Log($"{col.name} entered trigger");

        if (col.CompareTag("Telescope"))
        {
            zoomed = true;
            telescope = col.gameObject;

            /*if (telescope.TryGetComponent(out XRGrabInteractable grab))
            {
                telescopeGrab = grab;
            }
            else
            {
                Debug.LogWarning("Failed to get XRGrabInteractable");
            }*/
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

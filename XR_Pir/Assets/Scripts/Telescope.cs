using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class Telescope : MonoBehaviour
{
    [Header("Vignette")]
    [SerializeField] MeshRenderer vignette;
    [SerializeField] float vAppertureSizeMax = 0.8f, vAppertureSizeMin = 0.45f;
    float vApperture;

    [Header("Hand Transforms")]
    [SerializeField] Transform lHand;
    [SerializeField] Transform rHand;
    bool zoomed;

    [Header("Camera")]
    [SerializeField] Camera zoomCam;
    Camera mainCam;
    Transform zoomCamOrigin;

    [SerializeField] float maxZoomLength = 15f, zoomMultTest = 2f, zoomSmoothMove = 0.5f;
    float handDist;
    Vector3 velocity = Vector3.zero;

    [SerializeField] [Range(0.1f, 2f)] float camClipLength = 0.75f;

    GameObject telescope;
    MeshRenderer[] telescopeMeshes;
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
        if (telescopeInteractable.IsSelectedByLeft() && telescopeInteractable.IsSelectedByRight())
        {
            //held by both hands

            float dist = Vector3.Dot(lHand.position - rHand.position, transform.forward);
            handDist = Mathf.Abs(dist) * zoomMultTest;

            handDist = Remap(handDist, 0.1f, 1.2f, 0.9f, 1.1f);

            mainCam.enabled = false;
            zoomCam.enabled = true;

            //zoom camera forward, zoom length depending on how far the hands are away from eachother
            float zoomVal = Remap(handDist, 0.9f, 1.1f, 0f, maxZoomLength);

            
            Vector3 pos = Vector3.SmoothDamp(zoomCam.transform.position, ZoomCamWithoutClipping(mainCam.transform.position + (mainCam.transform.forward * zoomVal)), ref velocity, zoomSmoothMove);

            zoomCam.transform.SetPositionAndRotation(pos, mainCam.transform.rotation);

        }
        else if (telescopeInteractable.IsSelectedByLeft() || telescopeInteractable.IsSelectedByRight())
        {
            //held by one hand

            mainCam.enabled = false;
            zoomCam.enabled = true;

            Vector3 pos = Vector3.SmoothDamp(zoomCam.transform.position, ZoomCamWithoutClipping(mainCam.transform.position + (mainCam.transform.forward * maxZoomLength / 2)), ref velocity, zoomSmoothMove);

            zoomCam.transform.SetPositionAndRotation(pos, mainCam.transform.rotation);
        }


    }


    private void OnTriggerEnter(Collider col)
    {

        if (col.CompareTag("Telescope"))
        {
            zoomed = true;
            telescope = col.gameObject;
            telescopeMeshes = telescope.transform.parent.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer mr in telescopeMeshes)
            {
                mr.enabled = false;
            }

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

        if (col.CompareTag("Telescope"))
        {
            zoomed = false;
            telescope = null;
            telescopeGrab = null;

            foreach (MeshRenderer mr in telescopeMeshes)
            {
                mr.enabled = true;
            }

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

    Vector3 ZoomCamWithoutClipping(Vector3 zoomDesiredPos)
    {
        Vector3 startPos = telescope.transform.position;
        Vector3 dir = (zoomDesiredPos - startPos).normalized;
        float maxDist = Vector3.Distance(startPos, zoomDesiredPos);

        if(Physics.Raycast(startPos, dir, out RaycastHit hit, maxDist))
        {
            return hit.point - dir * camClipLength;
        }

        return zoomDesiredPos;
    }

    float Remap(float value, float a1, float a2, float b1, float b2)
    {
        return b1 + (value - a1) * (b2 - b1) / (a2 - a1);
    }

    private void OnDisable()
    {
        foreach (MeshRenderer mr in telescopeMeshes)
        {
            mr.enabled = true;
        }
    }

}

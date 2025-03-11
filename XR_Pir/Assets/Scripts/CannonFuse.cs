using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CannonFuse : MonoBehaviour
{
    bool fusePulled;
    Vector3 startPos;

    XRGrabInteractable grab;
    LoadCannon lc;

    private void Start()
    {
        startPos = transform.position;

        grab = GetComponent<XRGrabInteractable>();
        grab.enabled = false;

        lc = GetComponentInParent<LoadCannon>();
    }

    private void Update()
    {
        if (!fusePulled)
        {
            transform.position = startPos;
            fusePulled = false;
        }

        if (lc.isLoaded)
        {
            grab.enabled = true;
        }
    }


    public void SetFusePulled(bool pulled)
    {
        fusePulled = pulled;
    }
    public bool GetFusePulled()
    {
        return fusePulled;
    }
}

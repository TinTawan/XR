using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CannonFuse : MonoBehaviour
{
    bool fusePulled;
    [SerializeField] Transform startPos;

    XRGrabInteractable grab;
    LoadCannon lc;
    Rigidbody rb;

    private void Start()
    {
        //startPos = transform.position;

        grab = GetComponent<XRGrabInteractable>();
        grab.enabled = false;

        lc = GetComponentInParent<LoadCannon>();

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!fusePulled)
        {
            //transform.position = startPos;
            //rb.MovePosition(startPos);
            //rb.MoveRotation(lc.transform.rotation);
            fusePulled = false;
        }

        if (lc.isLoaded)
        {
            grab.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (!fusePulled)
        {
            rb.MovePosition(startPos.position);
            rb.MoveRotation(lc.transform.rotation);
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

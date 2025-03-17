using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CannonFuse : MonoBehaviour
{
    bool fusePulled;
    [SerializeField] Transform startPos;

    XRGrabInteractable grab;
    LoadCannon lc;
    Rigidbody rb;

    Collider fuseCol;

    private void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.enabled = false;

        fuseCol = GetComponent<Collider>();
        fuseCol.enabled = false;

        lc = GetComponentInParent<LoadCannon>();

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (lc.isLoaded)
        {
            grab.enabled = true;
            fuseCol.enabled = true;
        }

        if (!fusePulled)
        {
            transform.position = startPos.position;
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

        if (pulled)
        {
            Invoke(nameof(DisableFuse), 2f);
        }
    }
    public bool GetFusePulled()
    {
        return fusePulled;
    }

    void DisableFuse()
    {
        grab.enabled = false;
        fuseCol.enabled = false;
    }

}

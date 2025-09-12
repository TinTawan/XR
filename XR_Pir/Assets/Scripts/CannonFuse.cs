using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CannonFuse : MonoBehaviour
{
    bool fusePulled;
    [SerializeField] Transform startPos;

    XRGrabInteractable grab;
    XRBaseInteractable knob;

    LoadCannon lc;
    Rigidbody rb;

    Collider fuseCol;

    public event Action OnFusePulled;

    private void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.enabled = false;

        fuseCol = GetComponent<Collider>();
        fuseCol.enabled = false;

        lc = GetComponentInParent<LoadCannon>();

        rb = GetComponent<Rigidbody>();

        knob = transform.parent.GetComponent<XRBaseInteractable>();
    }

    private void Update()
    {
        if (lc.isLoaded)
        {
            grab.enabled = true;
            fuseCol.enabled = true;
        }
        else
        {
            DisableFuse();
        }

        if (fusePulled)
        {
            knob.enabled = false;

        }
        else
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

    public void FusePulled()
    {
        if (fusePulled) return;

        CancelInvoke(nameof(DisableFuse));
        CancelInvoke(nameof(EnableCannonPivot));
        CancelInvoke(nameof(ResetFusePulled));

        fusePulled = true;

        OnFusePulled?.Invoke();
        Debug.Log("[CannonFuse] Fuse pulled");

        Invoke(nameof(DisableFuse), 2f);
        Invoke(nameof(EnableCannonPivot), 2.5f);
        Invoke(nameof(ResetFusePulled), 3f);
        //ResetFusePulled();
    }
    /*public bool GetFusePulled()
    {
        return fusePulled;
    }*/

    void ResetFusePulled()
    {
        fusePulled = false;
    }

    void DisableFuse()
    {
        grab.enabled = false;
        fuseCol.enabled = false;

    }

    void EnableCannonPivot()
    {
        knob.enabled = true;

    }
}

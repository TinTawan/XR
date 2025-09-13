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
            IsFuseEnabled(true);

            //instead of EnableCannonPivot in Fire()
            knob.enabled = true;
        }
        else
        {
            IsFuseEnabled(false);

            knob.enabled = false;
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

        fusePulled = true;

        OnFusePulled?.Invoke();

        //DisableFuse();
        //Invoke(nameof(DisableFuse), 2f);
        //Invoke(nameof(EnableCannonPivot), 2.5f);
        //EnableCannonPivot();
        Invoke(nameof(ResetFusePulled), 1f);
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

    public void IsFuseEnabled(bool inBool)
    {
        grab.enabled = inBool;
        fuseCol.enabled = inBool;

    }

    public void EnableCannonPivot()
    {
        knob.enabled = true;

    }
}

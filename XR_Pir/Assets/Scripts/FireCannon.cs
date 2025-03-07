using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannon : MonoBehaviour
{
    LoadCannon lc;
    CannonFuse cf;

    GameObject cannonBall;
    [SerializeField] float fireStrength = 5f, fuseTime = 3f;
    [SerializeField] Transform firePoint;

    [SerializeField] private CannonTrajectory cannonTrajectory;

    private void Start()
    {
        lc = GetComponentInChildren<LoadCannon>();
        cf = GetComponentInChildren<CannonFuse>();

    }

    private void Update()
    {
        cannonTrajectory.ShowTrajectoryLine(firePoint.position, firePoint.forward * fireStrength);      // displays trajectory

        if (lc.GetIsLoaded())
        {
            cannonBall = lc.GetCannonBall();

            if (cf.GetFusePulled())
            {
                StartCoroutine(Fire());
                cf.SetFusePulled(false);

            }
        }
    }


    IEnumerator Fire()
    {
        Debug.Log("Fuse Pulled");

        yield return new WaitForSeconds(fuseTime);

        if (cannonBall != null)
        {
            Debug.Log("F I R E");
            Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            yield return new WaitForEndOfFrame();

            if (cannonBall.TryGetComponent(out SphereCollider col))
            {
                col.enabled = true;
            }
            else
            {
                Debug.LogWarning("Cant get collider");
            }

            yield return new WaitForEndOfFrame();

            rb.AddForce(firePoint.transform.forward * fireStrength, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(1f);

        Debug.Log("Reset");

        if (lc.TryGetComponent(out BoxCollider bc))
        {
            bc.enabled = true;
        }
        cannonBall = null;

    }
}

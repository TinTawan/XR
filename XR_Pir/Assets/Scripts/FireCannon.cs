using System.Collections;
using UnityEngine;

public class FireCannon : MonoBehaviour
{
    LoadCannon lc;
    CannonFuse cf;
    CannonTrajectory cannonTrajectory;

    GameObject cannonBall;
    Rigidbody baseRB;

    [SerializeField] float fireStrength = 5f, fuseTime = 3f, baseMoveSpeed = 0.5f;
    [SerializeField] Transform firePoint;

    [SerializeField] Transform baseLoadedPosition, baseBackFirePosition;
     

    private void Start()
    {
        lc = GetComponentInChildren<LoadCannon>();
        cf = GetComponentInChildren<CannonFuse>();
        cannonTrajectory = GetComponent<CannonTrajectory>();

        baseRB = GetComponentInChildren<Rigidbody>();

        baseRB.gameObject.transform.position = baseBackFirePosition.position;
    }

    private void Update()
    {
        if (lc.isLoaded)
        {
            //move forward to loaded position
            StartCoroutine(BaseMoveForward());

            // displays trajectory
            cannonTrajectory.EnableTrajectory(true);
            cannonTrajectory.ShowTrajectoryLine(firePoint.position, firePoint.forward * fireStrength);      

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
        lc.isLoaded = false;

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

            //fire cannonball in direction of cannon
            rb.AddForce(firePoint.transform.forward * fireStrength, ForceMode.Impulse);

            //move cannon base back
            baseRB.AddForce(Vector3.back * fireStrength, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(1f);

        Debug.Log("Reset");

        if (lc.TryGetComponent(out BoxCollider bc))
        {
            bc.enabled = true;
        }
        cannonBall = null;

        //lc.isLoaded = false;

        //disable trajectory
        cannonTrajectory.EnableTrajectory(false);

    }

    IEnumerator BaseMoveForward()
    {
        while(baseRB.transform.position.z > baseLoadedPosition.transform.position.z)
        {
            baseRB.transform.position += Vector3.forward * baseMoveSpeed;
            yield return null;
        }
    }
}

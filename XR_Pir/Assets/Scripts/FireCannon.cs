using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class FireCannon : MonoBehaviour
{
    LoadCannon loadCannon;
    CannonFuse cannonFuse;
    CannonTrajectory cannonTrajectory;

    GameObject thisCannonball;
    Rigidbody baseRB;

    [Header("Fire info")]
    [SerializeField] float fireStrength = 5f;
    [SerializeField] float fuseTime = 3f, baseMoveSpeed = 0.5f, baseBackFireStrength = 5f;
    [SerializeField] Transform firePoint;

    [Header("Move Positions")]
    [SerializeField] Transform baseLoadedPosition;
    [SerializeField] Transform baseBackFirePosition;

    [Header("Particle Effects")]
    [SerializeField] GameObject fusePS;
    [SerializeField] GameObject smokePS;
    [SerializeField] Transform fusePSPos, smokePSPos;

    [Header("Cannonball")]
    [SerializeField] GameObject cannonballPrefab;
    [SerializeField] Transform cannonballSpawnPos;


    bool isFiring = false;

    private void Start()
    {
        loadCannon = GetComponentInChildren<LoadCannon>();
        cannonFuse = GetComponentInChildren<CannonFuse>();
        cannonTrajectory = GetComponent<CannonTrajectory>();

        baseRB = GetComponentInChildren<Rigidbody>();

        baseRB.gameObject.transform.position = baseBackFirePosition.position;


        //Fire() using c# event rather than checking on Update()
        cannonFuse.OnFusePulled += DoFusePulled;
    }

    private void Update()
    {
        if (loadCannon.isLoaded)
        {
            //move forward to loaded position
            MoveBaseForward();

            // displays trajectory
            cannonTrajectory.EnableTrajectory(true);
            cannonTrajectory.ShowTrajectoryLine(firePoint.position, firePoint.forward * fireStrength);

            thisCannonball = loadCannon.GetCannonBall();

            /*if (cannonFuse.GetFusePulled())
            {
                StartCoroutine(Fire());
                cannonFuse.SetFusePulled(false);

                //Instantiate(fusePS, fusePSPos.position, Quaternion.identity, transform);
                ObjectPoolingManager.SpawnObject(fusePS, fusePSPos.position, Quaternion.identity);
            }*/
        }
    }

    void DoFusePulled()
    {
        if (!isFiring && loadCannon.isLoaded)
        {
            StartCoroutine(Fire());
            ObjectPoolingManager.SpawnObject(fusePS, fusePSPos.position, Quaternion.identity);
        }
    }

    /*IEnumerator Fire()
    {
        if (isFiring)
        {
            yield break;
        }
        isFiring = true;

        loadCannon.isLoaded = false;

        FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.CannonTriggered, transform.position, 0.1f);

        yield return new WaitForSeconds(fuseTime);

        if (thisCannonball != null)
        {
            thisCannonball.transform.SetParent(null);

            Rigidbody rb = thisCannonball.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            //yield return new WaitForEndOfFrame();

            if (thisCannonball.TryGetComponent(out SphereCollider col))
            {
                col.enabled = true;
            }

            FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.CannonLaunch, transform.position, 0.1f);
            ObjectPoolingManager.SpawnObject(smokePS, smokePSPos.position, Quaternion.identity);

            //yield return new WaitForEndOfFrame();

            //fire cannonball in direction of cannon
            rb.AddForce(firePoint.transform.forward * fireStrength, ForceMode.Impulse);

            //unload cannonball in LoadCannon
            loadCannon.isLoaded = false;
            yield return new WaitForEndOfFrame();
            loadCannon.UnloadCannonball();

            //move cannon base back
            baseRB.isKinematic = false;
            baseRB.AddForce(transform.forward * baseBackFireStrength, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(1f);

        if (loadCannon.TryGetComponent(out BoxCollider bc))
        {
            bc.enabled = true;
        }

        //disable trajectory
        cannonTrajectory.EnableTrajectory(false);
        baseRB.isKinematic = true;

        if (thisCannonball != null)
        {
            //after 5 seconds, return ball to pool
            yield return new WaitForSeconds(5f);
            ReturnCannonballToPool(thisCannonball);
        }


        //spawn new cannon ball from pool
        ObjectPoolingManager.SpawnObject(cannonballPrefab, cannonballSpawnPos.position, Quaternion.identity);

        isFiring = false;
    }*/

    IEnumerator Fire()
    {
        if (isFiring)
        {
            Debug.LogWarning("[FireCannon] Fire() called while already firing!");
            yield break;
        }
        isFiring = true;

        if (thisCannonball == null)
        {
            Debug.LogWarning("[FireCannon] Tried to fire but no cannonball assigned!");
            isFiring = false;
            yield break;
        }

        loadCannon.isLoaded = false; // stop LoadCannon.Update() from pinning the ball

        FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.CannonTriggered, transform.position, 0.1f);

        Debug.Log($"[FireCannon] Fuse lit for {thisCannonball.name} at {Time.time}");

        yield return new WaitForSeconds(fuseTime);

        if (thisCannonball != null)
        {
            // Detach from cannon
            thisCannonball.transform.SetParent(null);

            Rigidbody rb = thisCannonball.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            if (thisCannonball.TryGetComponent(out SphereCollider col))
                col.enabled = true;

            Debug.Log($"[FireCannon] Re-enabled physics on {thisCannonball.name} at {Time.time}");

            // Fire effects
            FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.CannonLaunch, transform.position, 0.1f);

            ObjectPoolingManager.SpawnObject(smokePS, smokePSPos.position, Quaternion.identity);

            // Fire instantly
            rb.AddForce(firePoint.forward * fireStrength, ForceMode.Impulse);
            Debug.Log($"[FireCannon] Force applied to {thisCannonball.name} at {Time.time}");

            // Cannon recoil
            baseRB.isKinematic = false;
            baseRB.AddForce(transform.forward * baseBackFireStrength, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("[FireCannon] Cannonball reference lost before firing!");
        }

        // Allow cannon collider again
        yield return new WaitForSeconds(1f);
        if (loadCannon.TryGetComponent(out BoxCollider bc))
            bc.enabled = true;

        // disable trajectory
        cannonTrajectory.EnableTrajectory(false);
        baseRB.isKinematic = true;

        // Pool ball after 5s if still valid
        if (thisCannonball != null)
        {
            GameObject firedBall = thisCannonball;
            thisCannonball = null; // clear our reference

            yield return new WaitForSeconds(5f);
            Debug.Log($"[FireCannon] Returning {firedBall.name} to pool at {Time.time}");
            ReturnCannonballToPool(firedBall);
        }

        // Spawn a fresh cannonball to grab
        ObjectPoolingManager.SpawnObject(cannonballPrefab, cannonballSpawnPos.position, Quaternion.identity);

        // Finished firing
        isFiring = false;
    }

    void MoveBaseForward()
    {
        baseRB.transform.position = Vector3.Lerp(baseRB.transform.position, baseLoadedPosition.position, baseMoveSpeed * Time.deltaTime);
    }


    void ReturnCannonballToPool(GameObject obj)
    {
        //reset necessary values
        obj.GetComponent<XRGrabInteractable>().enabled = true;
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.GetComponent<SphereCollider>().enabled = true;

        //and set it to null
        thisCannonball = null;

        ObjectPoolingManager.ReturnToPool(obj);
    }

    private void OnDisable()
    {
        cannonFuse.OnFusePulled -= DoFusePulled;
    }
}

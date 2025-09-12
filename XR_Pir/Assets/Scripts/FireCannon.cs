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

            //displays trajectory
            cannonTrajectory.EnableTrajectory(true);
            cannonTrajectory.ShowTrajectoryLine(firePoint.position, firePoint.forward * fireStrength);

            thisCannonball = loadCannon.GetCannonBall();
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
            yield break;
        }
        isFiring = true;

        if (thisCannonball == null)
        {
            isFiring = false;
            yield break;
        }

        loadCannon.isLoaded = false;

        FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.CannonTriggered, transform.position, 0.1f);

        yield return new WaitForSeconds(fuseTime);

        //cannonFuse.DisableFuse();

        if (thisCannonball != null)
        {
            //detach from cannon
            thisCannonball.transform.SetParent(null);

            Rigidbody rb = thisCannonball.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            if (thisCannonball.TryGetComponent(out SphereCollider col))
            {
                col.enabled = true;
            }

            FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.CannonLaunch, transform.position, 0.1f);
            ObjectPoolingManager.SpawnObject(smokePS, smokePSPos.position, Quaternion.identity);

            //fire cannon
            rb.AddForce(firePoint.forward * fireStrength, ForceMode.Impulse);

            //cannon kick back
            baseRB.isKinematic = false;
            baseRB.AddForce(transform.forward * baseBackFireStrength, ForceMode.Impulse);
        }

        //enable cannon collider
        yield return new WaitForSeconds(1f);
        if (loadCannon.TryGetComponent(out BoxCollider bc))
        {
            bc.enabled = true;
        }

        //disable trajectory
        cannonTrajectory.EnableTrajectory(false);
        baseRB.isKinematic = true;

        //pool cannonball if still valid
        if (thisCannonball != null)
        {
            GameObject firedBall = thisCannonball;
            thisCannonball = null; // clear our reference

            yield return new WaitForSeconds(5f);
            ReturnCannonballToPool(firedBall);
        }

        //fpawn new cannonball in barrel
        ObjectPoolingManager.SpawnObject(cannonballPrefab, cannonballSpawnPos.position, Quaternion.identity);

        //finished firing
        //cannonFuse.EnableCannonPivot();
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

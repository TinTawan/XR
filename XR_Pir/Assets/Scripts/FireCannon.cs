using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class FireCannon : MonoBehaviour
{
    LoadCannon lc;
    CannonFuse cf;
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
            MoveBaseForward();

            // displays trajectory
            cannonTrajectory.EnableTrajectory(true);
            cannonTrajectory.ShowTrajectoryLine(firePoint.position, firePoint.forward * fireStrength);      

            thisCannonball = lc.GetCannonBall();

            if (cf.GetFusePulled())
            {
                StartCoroutine(Fire());
                cf.SetFusePulled(false);

                //Instantiate(fusePS, fusePSPos.position, Quaternion.identity, transform);
                ObjectPoolingManager.SpawnObject(fusePS, fusePSPos.position, Quaternion.identity);
            }
        }
    }


    IEnumerator Fire()
    {
        lc.isLoaded = false;

        FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.CannonTriggered, transform.position, 0.1f);

        yield return new WaitForSeconds(fuseTime);

        if (thisCannonball != null)
        {
            Rigidbody rb = thisCannonball.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.CannonLaunch, transform.position, 0.1f);

            //Instantiate(smokePS, smokePSPos.position, Quaternion.identity, transform);
            ObjectPoolingManager.SpawnObject(smokePS, smokePSPos.position, Quaternion.identity);


            yield return new WaitForEndOfFrame();

            if (thisCannonball.TryGetComponent(out SphereCollider col))
            {
                col.enabled = true;
            }

            yield return new WaitForEndOfFrame();

            //fire cannonball in direction of cannon
            rb.AddForce(firePoint.transform.forward * fireStrength, ForceMode.Impulse);

            //move cannon base back
            baseRB.isKinematic = false;
            baseRB.AddForce(transform.forward * baseBackFireStrength, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(1f);


        if (lc.TryGetComponent(out BoxCollider bc))
        {
            bc.enabled = true;
        }
        //disable trajectory
        cannonTrajectory.EnableTrajectory(false);
        baseRB.isKinematic = true;

        //after 5 seconds, return ball to pool
        yield return new WaitForSeconds(5f);
        ReturnCannonballToPool(thisCannonball);
        

        //spawn new cannon ball from pool
        ObjectPoolingManager.SpawnObject(cannonballPrefab, cannonballSpawnPos.position, Quaternion.identity);

        

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
}

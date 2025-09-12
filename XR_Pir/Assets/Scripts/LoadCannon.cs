/*using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LoadCannon : MonoBehaviour
{
    [SerializeField] Transform ballPoint;

    public bool isLoaded { get; set; }
    GameObject thisCannonball;

    bool hasPlayed = false;       // checking for audio played before

    private void Start()
    {
        isLoaded = false;

    }

    private void Update()
    {
        if (isLoaded && thisCannonball != null)
        {
            thisCannonball.transform.position = ballPoint.position;

        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("CannonBall"))
        {
            GetComponent<BoxCollider>().enabled = false;

            isLoaded = true;
            thisCannonball = col.gameObject;

            if (thisCannonball.TryGetComponent(out XRGrabInteractable grab))
            {
                grab.enabled = false;

                //// audio for putting cannonball in cannon
                if (!hasPlayed)
                {
                    GameStateManager.Instance.CannonballPickedUp();
                    hasPlayed = true;
                }
            }

            thisCannonball.GetComponent<Rigidbody>().isKinematic = true;
            thisCannonball.GetComponent<Collider>().enabled = false;
            FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.CannonLoad, transform.position, 0.6f);

        }
    }



    public GameObject GetCannonBall()
    {
        return thisCannonball;
    }

    public void UnloadCannonball()
    {
        thisCannonball = null;
    }

}
*/
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LoadCannon : MonoBehaviour
{
    [SerializeField] Transform ballPoint;

    public bool isLoaded { get; set; }
    GameObject thisCannonball;

    bool hasPlayed = false; // audio flag

    private void Start()
    {
        isLoaded = false;
    }

    private void Update()
    {
        // only snap position while the ball is loaded
        if (isLoaded && thisCannonball != null)
        {
            thisCannonball.transform.position = ballPoint.position;
            thisCannonball.transform.rotation = ballPoint.rotation;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("CannonBall"))
        {
            GetComponent<BoxCollider>().enabled = false;

            isLoaded = true;
            thisCannonball = col.gameObject;

            Debug.Log("====================");
            Debug.Log($"[LoadCannon] {thisCannonball.name} loaded into cannon at {Time.time}");

            if (thisCannonball.TryGetComponent(out XRGrabInteractable grab))
            {
                grab.enabled = false;

                if (!hasPlayed)
                {
                    GameStateManager.Instance.CannonballPickedUp();
                    hasPlayed = true;
                }
            }

            Rigidbody rb = thisCannonball.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            Collider c = thisCannonball.GetComponent<Collider>();
            if (c != null) c.enabled = false;

            FindObjectOfType<AudioManager>().AudioTrigger(
                AudioManager.SoundFXCat.CannonLoad, transform.position, 0.6f);
        }
    }

    public GameObject GetCannonBall()
    {
        return thisCannonball;
    }

    public void UnloadCannonball()
    {
        if (thisCannonball != null)
        {
            Debug.Log($"[LoadCannon] Clearing reference to {thisCannonball.name} at {Time.time}");
        }

        thisCannonball = null;
        isLoaded = false;
    }
}

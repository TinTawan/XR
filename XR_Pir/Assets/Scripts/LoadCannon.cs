using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LoadCannon : MonoBehaviour
{
    [SerializeField] Transform ballPoint;

    public bool isLoaded { get; set; }
    GameObject thisCannonball;

    bool hasPlayed = false;

    private void Start()
    {
        isLoaded = false;
    }

    private void Update()
    {
        // only set position when ball is loaded
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

            Collider ballCol = thisCannonball.GetComponent<Collider>();
            if (ballCol != null) ballCol.enabled = false;


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
        isLoaded = false;
    }
}

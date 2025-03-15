using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class LoadCannon : MonoBehaviour
{
    [SerializeField] Transform ballPoint;

    public bool isLoaded { get; set; }
    GameObject cannonBall;


    private void Start()
    {
        isLoaded = false;

    }

    private void Update()
    {
        if (isLoaded)
        {
            cannonBall.transform.position = ballPoint.position;

        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("CannonBall"))
        {
            GetComponent<BoxCollider>().enabled = false;

            isLoaded = true;
            cannonBall = col.gameObject;

            if (cannonBall.TryGetComponent(out XRGrabInteractable grab))
            {
                grab.enabled = false;
            }
            else
            {
                Debug.LogWarning("Cant get grab");
            }

            //cannonBall.transform.position = ballPoint.position;
            cannonBall.GetComponent<Rigidbody>().isKinematic = true;
            cannonBall.GetComponent<Collider>().enabled = false;
            FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.CannonLoad, transform.position, 0.6f);

            Debug.Log("Cannon Loaded!");
        }
    }



    public GameObject GetCannonBall()
    {
        return cannonBall;
    }

}

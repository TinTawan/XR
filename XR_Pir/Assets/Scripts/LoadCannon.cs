using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class LoadCannon : MonoBehaviour
{
    [SerializeField] Transform ballPoint;

    public bool isLoaded { get; set; }
    GameObject thisCannonball;


    private void Start()
    {
        isLoaded = false;

    }

    private void Update()
    {
        if (isLoaded)
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
            }
            else
            {
                Debug.LogWarning("Cant get grab");
            }

            //cannonBall.transform.position = ballPoint.position;
            thisCannonball.GetComponent<Rigidbody>().isKinematic = true;
            thisCannonball.GetComponent<Collider>().enabled = false;
            FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.CannonLoad, transform.position, 0.6f);

            Debug.Log("Cannon Loaded!");
        }
    }



    public GameObject GetCannonBall()
    {
        return thisCannonball;
    }

}

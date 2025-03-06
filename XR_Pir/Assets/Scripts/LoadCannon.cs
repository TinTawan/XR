using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class LoadCannon : MonoBehaviour
{
    [SerializeField] Transform ballPoint;

    bool isLoaded;
    GameObject cannonBall;


    private void Start()
    {
        isLoaded = false;

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

            cannonBall.transform.position = ballPoint.position;
            cannonBall.GetComponent<Rigidbody>().isKinematic = true;
            cannonBall.GetComponent<Collider>().enabled = false;

            Debug.Log("Cannon Loaded!");
        }
    }


    public bool GetIsLoaded()
    {
        return isLoaded;
    }

    public GameObject GetCannonBall()
    {
        return cannonBall;
    }

}

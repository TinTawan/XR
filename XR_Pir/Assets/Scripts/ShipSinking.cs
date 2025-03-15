using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSinking : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
        {
            GetComponent<ShipPath>().StopMovement();
            GetComponent<Animator>().SetTrigger("StartSinking");
        }
    }
}
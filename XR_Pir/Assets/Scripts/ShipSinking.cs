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
            FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.EnemyHit, transform.position, 1f);
            GetComponent<Animator>().SetTrigger("StartSinking");
        }
    }
}
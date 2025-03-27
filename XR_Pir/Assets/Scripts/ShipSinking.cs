using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSinking : MonoBehaviour
{
    private bool oneShipHitPlayed = false;
    private bool twoShipsHitPlayed = false;
    private bool threeShipsHitPlayed = false;

    private static int shipsHitCount = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
        {
            shipsHitCount++;

            GetComponent<ShipPath>().StopMovement();

            if (shipsHitCount == 1 && !oneShipHitPlayed)
            {
                GameStateManager.Instance.OneShipHit();
                oneShipHitPlayed = true;
            }
            else if (shipsHitCount == 2 && !twoShipsHitPlayed)
            {
                GameStateManager.Instance.TwoShipsHit();
                twoShipsHitPlayed = true;
            }
            else if (shipsHitCount == 3 && !threeShipsHitPlayed)
            {
                GameStateManager.Instance.ThreeShipsHit();
                threeShipsHitPlayed = true;
            }

            GetComponent<Animator>().SetTrigger("StartSinking");
            FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.EnemyHit, transform.position, 0.6f);
        }
    }
}
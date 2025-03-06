using UnityEngine;

public class TestDestroyable : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("CannonBall"))
        {
            Destroy(gameObject, 0.1f);
        }
    }
}

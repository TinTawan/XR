using UnityEngine;

public class ReturnParticleToPool : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        ObjectPoolingManager.ReturnToPool(gameObject);
    }
}

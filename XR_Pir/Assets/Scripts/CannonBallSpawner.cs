using System.Collections;
using UnityEngine;

public class CannonBallSpawner : MonoBehaviour
{
    [SerializeField] GameObject cannonballPrefab;
    [SerializeField] Transform spawnpoint;

    Bounds colBounds;

    private void Start()
    {
        colBounds = spawnpoint.GetComponent<BoxCollider>().bounds;
        StartCoroutine(SpawnCannonBall());
    }

    Vector3 GetRandPosInBounds(Bounds b)
    {
        return new(Random.Range(b.min.x, b.max.x), Random.Range(b.min.y, b.max.y), Random.Range(b.min.z, b.max.z));
    }

    IEnumerator SpawnCannonBall()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(.3f);
            ObjectPoolingManager.SpawnObject(cannonballPrefab, GetRandPosInBounds(colBounds), Quaternion.identity);

        }
    }
}

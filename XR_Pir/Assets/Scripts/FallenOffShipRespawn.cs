using System.Collections;
using UnityEngine;

public class FallenOffShipRespawn : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;
    [SerializeField] MeshRenderer vignette;

    float vApperture, respawnTime = 1f;

    private void Start()
    {
        vignette.materials[0].SetFloat("_ApertureSize", 1);
        vignette.enabled = false;

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            //do vignette 
            StartCoroutine(AppertureZoomIn(0.05f));

            StartCoroutine(Respawn(col.gameObject, respawnTime));
        }

        if (col.CompareTag("Telescope"))
        {
            StartCoroutine(Respawn(col.transform.parent.gameObject, respawnTime));

        }

    }

    IEnumerator Respawn(GameObject go, float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);
        StartCoroutine(AppertureZoomOut(0.1f));

        yield return new WaitForEndOfFrame();
        go.transform.position = respawnPoint.position;


    }

    IEnumerator AppertureZoomIn(float delta)
    {
        Debug.Log("zoom");

        vignette.materials[0].SetFloat("_ApertureSize", 1);
        vignette.enabled = true;

        while (vApperture > 0)
        {
            vApperture -= delta;

            vignette.materials[0].SetFloat("_ApertureSize", vApperture);
            yield return null;
        }

    }

    IEnumerator AppertureZoomOut(float delta)
    {
        while (vApperture < 1)
        {
            vApperture += delta;

            vignette.materials[0].SetFloat("_ApertureSize", vApperture);
            yield return null;
        }

        vignette.enabled = false;

    }
}

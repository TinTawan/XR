using UnityEngine;

public class ControlAudio : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip myClip;
    [SerializeField] float volume = 1f;

    public void StartAudio() // plays sound effects stored in audioSource
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = myClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    void Update() // destroys gameObject
    {
        if (!audioSource.isPlaying)
        {
            //Destroy(gameObject);
            ObjectPoolingManager.ReturnToPool(gameObject);
        }
    }

    public void SetClip(AudioClip inClip)
    {
        myClip = inClip;
    }

    public void SetVol(float inVol)
    {
        volume = inVol;
    }
}
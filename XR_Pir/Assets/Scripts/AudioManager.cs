using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundFXCat { CannonLaunch, CannonLoad, CannonTriggered, EnemyHit }

    public GameObject audioObject;
    public AudioClip[] launchClips;
    public AudioClip[] loadClips;
    public AudioClip[] triggeredClips;
    public AudioClip[] enemyHitClips;

    public void AudioTrigger(SoundFXCat audioType, Vector3 audioPosition, float volume)
    {
        GameObject newAudio = GameObject.Instantiate(audioObject, audioPosition, Quaternion.identity);
        ControlAudio ca = newAudio.GetComponent<ControlAudio>();
        switch (audioType)
        {
            case (SoundFXCat.CannonLaunch):   // sound effect for cannonball launching from cannon
                ca.myClip = launchClips[Random.Range(0, launchClips.Length)];
                break;
            case (SoundFXCat.CannonLoad):   // sound effect for loading cannonball into cannon
                ca.myClip = loadClips[Random.Range(0, loadClips.Length)];
                break;
            case (SoundFXCat.CannonTriggered):   // sound effect for cannon being triggered to launch
                ca.myClip = triggeredClips[Random.Range(0, triggeredClips.Length)];
                break;
            case (SoundFXCat.EnemyHit):   // sound effect for enemy ship being hit
                ca.myClip = enemyHitClips[Random.Range(0, enemyHitClips.Length)];
                break;
        }
        ca.volume = volume;
        ca.StartAudio();
    }
}
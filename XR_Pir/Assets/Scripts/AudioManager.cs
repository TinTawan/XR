using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundFXCat 
    { 
        CannonLaunch, 
        CannonLoad, 
        CannonTriggered, 
        EnemyHit 
    }

    public GameObject audioObject;

    [SerializeField] AudioClip[] launchClips;
    [SerializeField] AudioClip[] loadClips;
    [SerializeField] AudioClip[] triggeredClips;
    [SerializeField] AudioClip[] enemyHitClips;

    public void AudioTrigger(SoundFXCat audioType, Vector3 audioPosition, float volume)
    {
        //GameObject newAudio = GameObject.Instantiate(audioObject, audioPosition, Quaternion.identity);
        GameObject newAudio = ObjectPoolingManager.SpawnObject(audioObject, audioPosition, Quaternion.identity);

        ControlAudio ca = newAudio.GetComponent<ControlAudio>();
        switch (audioType)
        {
            case (SoundFXCat.CannonLaunch):   // sound effect for cannonball launching from cannon
                ca.SetClip(launchClips[Random.Range(0, launchClips.Length)]);
                break;
            case (SoundFXCat.CannonLoad):   // sound effect for loading cannonball into cannon
                ca.SetClip(loadClips[Random.Range(0, loadClips.Length)]);
                break;
            case (SoundFXCat.CannonTriggered):   // sound effect for cannon being triggered to launch
                ca.SetClip(triggeredClips[Random.Range(0, triggeredClips.Length)]);
                break;
            case (SoundFXCat.EnemyHit):   // sound effect for enemy ship being hit
                ca.SetClip(enemyHitClips[Random.Range(0, enemyHitClips.Length)]);
                break;
        }
        ca.SetVol(volume);
        ca.StartAudio();
    }
}
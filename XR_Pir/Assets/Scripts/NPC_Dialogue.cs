using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialogue : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] DialogueLines;
    private bool TelescopeAudioPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SceneLoaded());
    }

    IEnumerator SceneLoaded()
    {
        yield return new WaitForSeconds(3f);    // allows the scene to load before playing voiceline
        TriggerNPCDialogue(0);      // plays greeting dialogue
    }

    public void TriggerNPCDialogue(int index)
    {
        if (index == 1 && TelescopeAudioPlayed)
            return;
        
        audioSource.clip = DialogueLines[index];
        audioSource.Play();

        if (index == 1)
            TelescopeAudioPlayed = true;    // stops telescope audio playing repeatedly
    }
}

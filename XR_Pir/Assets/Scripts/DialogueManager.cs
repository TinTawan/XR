using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateDialogue
{
    public GameState gameState;
    public AudioClip audioClip;
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public AudioSource audioSource;
    public List<StateDialogue> stateDialogue;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayStateDialogue(GameState state)
    {
        foreach (var pair in stateDialogue)
        {
            if (pair.gameState == state)
            {
                PlayDialogue(pair.audioClip);
                return;
            }
        }
    }

    public void PlayDialogue(AudioClip clip)
    {
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
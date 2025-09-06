using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_MoveToPlayer : MonoBehaviour
{
    [SerializeField] Transform player, npc;

    [SerializeField] List<Transform> upperDeckTransforms = new List<Transform>(), lowerDeckTransforms = new List<Transform>();
    [SerializeField] GameObject upperDeckPositions, lowerDeckPositions;
    Transform startTransform;


    List<Transform> enabledDeckTransforms;

    private void Start()
    {
        startTransform = upperDeckTransforms[0];
        npc.transform.position = startTransform.position;

    }

    private void Update()
    {
        if (player.position.y > 8)
        {
            EnableDeck(0);
        }
        else
        {
            EnableDeck(1);
        }
    }


    //enable which set of positions should be active: 0 = Uppder deck, 1 = Lower Deck
    void EnableDeck(int deckIndex)
    {
        switch (deckIndex)
        {
            case 0:
                enabledDeckTransforms = new List<Transform>();

                for(int i = 0; i < upperDeckTransforms.Count; i++)
                {
                    enabledDeckTransforms.Add(upperDeckTransforms[i]);
                }
                break;
            case 1:
                enabledDeckTransforms = new List<Transform>();

                for (int i = 0; i < lowerDeckTransforms.Count; i++)
                {
                    enabledDeckTransforms.Add(lowerDeckTransforms[i]);
                }
                break;
            default:
                enabledDeckTransforms = new List<Transform>();

                for (int i = 0; i < upperDeckTransforms.Count; i++)
                {
                    enabledDeckTransforms.Add(upperDeckTransforms[i]);
                }
                break;
        }
    }

    Transform ShortestDistance(List<Transform> enabledTransforms)
    {
        Transform closestItem = null;
        float shortest = Mathf.Infinity;

        foreach(Transform item in enabledTransforms)
        {
            //Vector3.Distance(player.position, item.position);
            shortest = Mathf.Min(shortest, Vector3.Distance(player.position, item.position));
            closestItem = item;
        }

        return closestItem;
    }
}

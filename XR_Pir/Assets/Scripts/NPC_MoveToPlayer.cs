using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_MoveToPlayer : MonoBehaviour
{
    [SerializeField] Transform player, npc;

    [SerializeField] List<Transform> upperDeckTransforms = new List<Transform>(), lowerDeckTransforms = new List<Transform>();
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

        npc.transform.position = ReturnNPCPosition(enabledDeckTransforms);

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

    Vector3 ReturnNPCPosition(List<Transform> enabledTransforms)
    {
        Transform closestItem = null;
        float minDist = Mathf.Infinity;

        foreach (Transform item in enabledTransforms)
        {
            float distance = Vector3.Distance(item.position, player.position);

            if (distance < minDist)
            {
                closestItem = item;
                minDist = distance;
                Debug.Log($"closest item: {closestItem.gameObject.name} distance: {distance}");
            }
        }

        return closestItem.position;
    }

}

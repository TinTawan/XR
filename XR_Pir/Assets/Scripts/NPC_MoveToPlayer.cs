using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_MoveToPlayer : MonoBehaviour
{
    [SerializeField] Transform player, npc;

    [SerializeField] List<Transform> npcTransforms;
    [SerializeField] GameObject upperDeckPositions, lowerDeckPositions;
    Transform startTransform;



    private void Start()
    {
        startTransform = npcTransforms[0];
        npc.transform.position = startTransform.position;
    }

    private void Update()
    {
        if(player.position.y > 8)
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
                upperDeckPositions.SetActive(true);
                lowerDeckPositions.SetActive(false);
                break;
            case 1:
                upperDeckPositions.SetActive(false);
                lowerDeckPositions.SetActive(true);
                break;
            default:
                upperDeckPositions.SetActive(true);
                lowerDeckPositions.SetActive(false);
                break;
        }
    }
}

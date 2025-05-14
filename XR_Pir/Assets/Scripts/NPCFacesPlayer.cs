using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFacesPlayer : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDirection = player.position - transform.position;
        Quaternion facingRotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, facingRotation, Time.deltaTime * 5f);
    }
}
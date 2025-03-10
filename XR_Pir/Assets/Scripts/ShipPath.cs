using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPath : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float radius = 5f;
    private float angle = 0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;      // get current pos of ship
    }

    void Update()
    {
        angle += speed * Time.deltaTime;    // updates angle

        float xPos = Mathf.Cos(angle) * radius + startPos.x;     // calc x position
        float zPos = Mathf.Sin(angle) * radius + startPos.z;     // calc z position

        transform.position = new Vector3(xPos, transform.position.y, zPos);             // moves ship in a circle
        Vector3 direction = new Vector3(-Mathf.Sin(angle), 0f, Mathf.Cos(angle));       // get direction the ship should face
        transform.rotation = Quaternion.LookRotation(direction);                        // rotate ship to face forward
    }
}
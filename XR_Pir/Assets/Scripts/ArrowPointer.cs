using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public float distance;
    public float speed;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = startingPos;
        v.y += distance * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }
}
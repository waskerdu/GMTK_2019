using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.left * 10;
    }
}

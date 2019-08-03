using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float damageSoak = 50f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Shield: Collision detected!");
    }
}

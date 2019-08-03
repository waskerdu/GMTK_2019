using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmCall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Swarm Call Triggered");
        if (!gameObject.activeInHierarchy) return;

        if (collision.GetComponent<Enemy>())
        {
            Debug.Log("FoundEnemy");
            collision.SendMessage("Swarm", GetComponentInParent<Enemy>().gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Swarm Call Triggered");
        if (!gameObject.activeInHierarchy) return;

        if (collision.gameObject.GetComponent<Enemy>())
        {
            Debug.Log("FoundEnemy");
            collision.gameObject.SendMessage("Swarm", GetComponentInParent<Enemy>().gameObject);
        }
    }
}

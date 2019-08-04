using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmCall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy) return;

        if (collision.GetComponent<Enemy>())
        {
            collision.SendMessage("Swarm", GetComponentInParent<Enemy>().gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameObject.activeInHierarchy) return;

        if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.SendMessage("Swarm", GetComponentInParent<Enemy>().gameObject);
        }
    }

    public void DamageEnemy(float damage)
    {
        return;
    }
}

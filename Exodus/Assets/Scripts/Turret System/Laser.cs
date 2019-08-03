using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Bullet
{
    public float hitDetectTimeout = 0.3f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(string.Format("I've triggered on {0}! Dealing {1} damage.", collision.name, damage));
        StartCoroutine(IgnoreForTime(collision, GetComponent<Collider2D>(), hitDetectTimeout));
    }

    IEnumerator IgnoreForTime(Collider2D collider1, Collider2D collider2, float time)
    {
        Physics2D.IgnoreCollision(collider1, collider2);
        yield return new WaitForSeconds(time);
        Physics2D.IgnoreCollision(collider1, collider2, false);
    }
}

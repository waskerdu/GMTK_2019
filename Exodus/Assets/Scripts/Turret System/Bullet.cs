using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float maxLifetime = 5f;
    [HideInInspector] public float damage;
    public bool pierce;

    float lifeTime;

    private void OnEnable()
    {
        lifeTime = maxLifetime;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.Find("Player").GetComponentInChildren<Collider2D>());
    }

    protected virtual void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);

        Deteriorate();
    }

    protected void Deteriorate()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            TurretManager.Instance.bulletPooler.Push(this);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(string.Format("I've triggered on {0}! Dealing {1} damage.", collision.name, damage));
        if (pierce)
            Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
        else
            TurretManager.Instance.bulletPooler.Push(this);
    }
}

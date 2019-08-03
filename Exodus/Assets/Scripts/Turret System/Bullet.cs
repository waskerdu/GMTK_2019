﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float maxLifetime = 5f;
    [HideInInspector] public float damage;

    float lifeTime;

    private void OnEnable()
    {
        lifeTime = maxLifetime;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            TurretManager.Instance.bulletPooler.Push(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(string.Format("I've triggered on {0}! Dealing {1} damage.", collision.name, damage));
        TurretManager.Instance.bulletPooler.Push(this);
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    [HideInInspector] public Transform target;
    public float turnSpeed = 0.2f;
    
    protected override void Update()
    {
        if (target != null)
        {
            //rotate towards target
            float dot = Vector3.Dot(transform.right, transform.position - target.position);

            dot = dot > 0 ? Mathf.Max(0.1f, dot) : dot < 0 ? Mathf.Min(-0.1f, dot) : 0;
            transform.Rotate(new Vector3(0, 0, dot * turnSpeed * Time.deltaTime));
        }

        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        Deteriorate();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Debug.Log(string.Format("I've triggered on {0}! Dealing {1} damage.", collision.name, damage));
            TurretManager.Instance.rocketPooler.Push(this);
            collision.SendMessageUpwards("DamageEnemy", damage);
        }
    }
}

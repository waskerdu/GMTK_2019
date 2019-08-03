using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum MovementMode {Wander, Beeline};
    Vector3 planetPos = new Vector3(0,0,0);

    [SerializeField] float health = 3f;
    [SerializeField] float damage = 1f;
    [SerializeField] bool isSwarmKing = false;
    [SerializeField] bool isSwarmDrone = false;
    [Header("Movement")]
    [SerializeField] MovementMode movementMode = MovementMode.Wander;
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float newWanderDirectionTime = 3f;
    [SerializeField] float wanderAccuracyAdjust = 4f;
    public Vector3 targetDir = new Vector3();
    Rigidbody2D rb;
    float directionChangeTimer = 0f;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void GameOver()
    {
        movementSpeed *= 1.8f;
        movementMode = MovementMode.Beeline;
        isSwarmDrone = false;
        isSwarmKing = false;
    }


    void Move()
    {
        switch (movementMode)
        {
            case MovementMode.Wander:
                ChangeTargetDir();
                break;
                

            case MovementMode.Beeline:
                targetDir = (planetPos - transform.position).normalized;
                break;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, targetDir), Time.deltaTime * rotateSpeed);
        rb.velocity = transform.up * movementSpeed;
    }

    private void ChangeTargetDir()
    {
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer < 0)
        {
            directionChangeTimer = newWanderDirectionTime;
            targetDir = (planetPos - transform.position).normalized;
            targetDir.x += UnityEngine.Random.Range(-wanderAccuracyAdjust, wanderAccuracyAdjust);
            targetDir.y += UnityEngine.Random.Range(-wanderAccuracyAdjust, wanderAccuracyAdjust);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SendMessageUpwards("SmallAttackSound");
        collision.gameObject.SendMessage("DamagePlanet", damage);
        Die();
    }

    public void DamageEnemy(float damage)
    {
        SendMessageUpwards("SmallDamageSound");
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SendMessageUpwards("SmallDeathSound");
        gameObject.SetActive(false);
    }
}

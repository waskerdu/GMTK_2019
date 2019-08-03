using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum MovementMode {Wander, Beeline, Swarm};
    Vector3 planetPos = new Vector3(0,0,0);

    [SerializeField] float health = 3f;
    [SerializeField] float damage = 1f;
    [SerializeField] bool isSwarmKing = false;
    [SerializeField] float kingScale = 1.5f;
    [SerializeField] float kingDamage = 3f;
    [SerializeField] float swarmDistance = 5f;
    [Header("Movement")]
    [SerializeField] MovementMode movementMode = MovementMode.Wander;
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float kingSpeed = 0.7f;
    [SerializeField] float swarmSpeed = 1.6f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float swarmRotateSpeed = 1.8f;
    [SerializeField] float newWanderDirectionTime = 3f;
    [SerializeField] float wanderAccuracyAdjust = 4f;
    [SerializeField] float beelineDistance = 4f;
    public Vector3 targetDir = new Vector3();
    Rigidbody2D rb;
    float directionChangeTimer = 0f;
    GameObject swarmKing;
    Vector3 originalScaling;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScaling = transform.localScale;
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
        isSwarmKing = false;
    }

    void CheckDistanceToPlanet()
    {
        if ((planetPos - transform.position).magnitude < beelineDistance)
        {
            movementMode = MovementMode.Beeline;
        }

        else if ((planetPos - transform.position).magnitude < swarmDistance && isSwarmKing)
        {
            SendMessageUpwards("SwarmSound");

            GetComponentInChildren<SwarmCall>(true).gameObject.SetActive(true);
        }
        

    }


    void Move()
    {
        switch (movementMode)
        {
            case MovementMode.Wander:
                ChangeTargetDir();
                CheckDistanceToPlanet();
                break;


            case MovementMode.Beeline:
                targetDir = (planetPos - transform.position).normalized;
                break;

            case MovementMode.Swarm:
                CheckDistanceToPlanet();
                targetDir = (swarmKing.transform.position - transform.position).normalized;
                break;
        }
        SetRotation();
        SetVelocity();

    }

    private void SetRotation()
    {
        if (movementMode == MovementMode.Swarm)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, targetDir), Time.deltaTime * swarmRotateSpeed);

        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, targetDir), Time.deltaTime * rotateSpeed);

        }
    }

    private void SetVelocity()
    {
        if (isSwarmKing)
        {
            rb.velocity = transform.up * kingSpeed;
        }
        else if (movementMode == MovementMode.Swarm)
        {
            rb.velocity = transform.up * swarmSpeed;
        }
        else
        {
            rb.velocity = transform.up * movementSpeed;
        }
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
        if (isSwarmKing)
        {
            SendMessageUpwards("BigAttackSound");
            
            collision.gameObject.SendMessageUpwards("DamagePlanet", kingDamage);
        }
        else
        {
            SendMessageUpwards("SmallAttackSound");
            collision.gameObject.SendMessageUpwards("DamagePlanet", damage);

        }


        Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            return;
        }
        if (isSwarmKing)
        {
            SendMessageUpwards("BigAttackSound");

            collision.gameObject.SendMessageUpwards("DamagePlanet", kingDamage);
        }
        else
        {
            SendMessageUpwards("SmallAttackSound");
            collision.gameObject.SendMessageUpwards("DamagePlanet", damage);

        }
    }


    public void DamageEnemy(float damage)
    {
        if (isSwarmKing)
        {
            SendMessageUpwards("BigDamageSound");

        }
        else
        {
            SendMessageUpwards("SmallDamageSound");

        }
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isSwarmKing)
        {
            SendMessageUpwards("BigDeathSound");

        }
        else
        {
            SendMessageUpwards("SmallDeathSound");

        }
        swarmKing = null;
        isSwarmKing = false;
        transform.localScale = originalScaling;
        movementMode = MovementMode.Wander;
        GetComponentInChildren<SwarmCall>(true).gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Swarm(GameObject king)
    {
        if (isSwarmKing || swarmKing != null)
        {
            return;
        }

        swarmKing = king;
        movementMode = MovementMode.Swarm;
    }

    public void BecomeKing()
    {
        isSwarmKing = true;
        transform.localScale = new Vector3(kingScale,kingScale,kingScale);
    }
}

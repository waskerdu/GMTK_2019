using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum MovementMode {Wander, Beeline};
    Vector3 planetPos = new Vector3(0,0,0);
    [SerializeField] MovementMode movementMode = MovementMode.Wander;
    [SerializeField] bool isSwarmKing = false;
    [SerializeField] bool isSwarmDrone = false;
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float newWanderDirectionTime = 3f;
    [SerializeField] float wanderAccuracyAdjust = 4f;
    public Vector3 targetDir = new Vector3();
    Rigidbody2D rigidbody;
    float directionChangeTimer = 0f;
    // Start is called before the first frame update

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void GameOver()
    {
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
        rigidbody.velocity = transform.up * movementSpeed;
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
}

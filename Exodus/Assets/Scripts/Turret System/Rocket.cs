using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    [HideInInspector] public Transform target;
    public Transform aimLocation;
    public float turnSpeed = 0.2f;

    protected override void Update()
    {
        if (target != null)
        {
            //rotate towards target
            aimLocation.LookAt(target.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, aimLocation.rotation, turnSpeed);
        }

        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        Deteriorate();
    }
}

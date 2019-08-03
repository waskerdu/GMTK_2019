using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTurret : MonoBehaviour
{
    [HideInInspector] public Transform targetPosition;
    [HideInInspector] public Transform topPosition;
    public LayerMask turretPositionMask;
    public float detectionRadius = 0.5f;

    void Update()
    {
        RaycastHit2D[] hitinfo = Physics2D.CircleCastAll(topPosition.position, detectionRadius, Vector2.up, 0.01f, turretPositionMask);
        if (hitinfo.Length > 0)
        {
            Transform closest = hitinfo[0].transform;
            float distance = Vector2.Distance(topPosition.position, closest.position);

            for (int i = 1; i < hitinfo.Length; i++)
            {
                float newDist = Vector2.Distance(topPosition.position, hitinfo[i].transform.position);
                if (newDist < distance)
                {
                    distance = newDist;
                    closest = hitinfo[i].transform;
                }
            }

            targetPosition = closest;
        }

        if (targetPosition != null)
        {
            transform.position = targetPosition.position;
            transform.rotation = targetPosition.rotation;
        }
    }
}

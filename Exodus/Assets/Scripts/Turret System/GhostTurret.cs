using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTurret : MonoBehaviour
{
    [HideInInspector] public TurretPosition targetPosition;
    [HideInInspector] public Transform topPosition;
    public LayerMask turretPositionMask;
    public float detectionRadius = 0.5f;
    [HideInInspector] public float turretHeightOffset = 1f;

    void Update()
    {
        targetPosition = TurretManager.Instance.GetTopPosition();

        if (targetPosition != null)
        {
            transform.position = targetPosition.positionObject.transform.position + transform.up * turretHeightOffset * targetPosition.turrets.Count;
            transform.rotation = targetPosition.positionObject.transform.rotation;
        }
    }
}

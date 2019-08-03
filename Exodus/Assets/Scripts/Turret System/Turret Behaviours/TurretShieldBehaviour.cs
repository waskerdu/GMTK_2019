using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Shield Behaviour", menuName = "Turret/Shield Behaviour")]
public class TurretShieldBehaviour : TurretBehaviour
{
    public Shield shieldPrefab;

    Shield shield;

    public override void Init()
    {
        shield = Instantiate(shieldPrefab, turret.transform);
        shield.transform.position = turret.bulletSpawnPoint.position;
    }

    public override void Shutdown()
    {
        Destroy(shield.gameObject);
        shield = null;
    }
}

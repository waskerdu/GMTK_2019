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
        shield.behaviour = this;
        shield.transform.position = turret.bulletSpawnPoint.position;
    }

    public void DestroyTurret()
    {
        turret.DestroyTurret();
    }

    public override void Shutdown()
    {
        Destroy(shield.gameObject);
        shield = null;
    }
}

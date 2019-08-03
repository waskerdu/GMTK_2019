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
        TurretSoundManager.Instance.PlaySound("ShieldOn");
    }

    public void DestroyTurret()
    {
        turret.DestroyTurret();
        TurretSoundManager.Instance.PlaySound("ShieldOff", true);
    }

    public override void Shutdown()
    {
        Destroy(shield.gameObject);
        TurretSoundManager.Instance.PlaySound("ShieldOff", true);
        shield = null;
    }
}

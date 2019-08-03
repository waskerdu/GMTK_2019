using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Shoot Behaviour", menuName = "Turret/Shoot Behaviour")]
public class TurretShootBehaviour : TurretBehaviour
{
    public float fireCoolDown = 0.5f;
    public float bulletDamage = 13.5f;

    float timeSinceLastFire = 0;

    public override void Update()
    {
        if (timeSinceLastFire >= fireCoolDown)
        {
            timeSinceLastFire = 0;
            Fire();
        }
        else
            timeSinceLastFire += Time.deltaTime;
    }

    public void Fire()
    {
        Bullet bullet = TurretManager.Instance.bulletPooler.Pop();
        bullet.damage = bulletDamage * (1 + turret.GetBoostMultiplier());
        bullet.transform.parent = null;
        bullet.transform.position = turret.transform.position;
        bullet.transform.rotation = turret.transform.rotation;
    }
}

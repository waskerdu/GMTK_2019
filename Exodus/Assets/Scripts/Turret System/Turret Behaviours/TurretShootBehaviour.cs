﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Shoot Behaviour", menuName = "Turret/Shoot Behaviour")]
public class TurretShootBehaviour : TurretBehaviour
{
    public enum BulletType
    {
        Bullet,
        Laser,
        Rocket
    }

    public float fireCoolDown = 0.5f;
    public float bulletDamage = 13.5f;
    public BulletType bulletType;
    public bool trackTarget;

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
        Bullet bullet;

        if (bulletType == BulletType.Bullet)
        {
            bullet = TurretManager.Instance.bulletPooler.Pop();
            bullet.transform.parent = null;
        }
        else if (bulletType == BulletType.Rocket)
        {
            Rocket rocket = TurretManager.Instance.rocketPooler.Pop() as Rocket;
            rocket.target = GameObject.Find("Damage Test").transform;
            bullet = rocket;
            bullet.transform.parent = null;
        }
        else
        {
            bullet = TurretManager.Instance.laserPooler.Pop();
            bullet.transform.parent = turret.transform;
        }

        bullet.damage = bulletDamage * (1 + turret.GetBoostMultiplier());
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;
    }
}

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
    public LayerMask enemyMask;
    public float BoxCheckRadius = 2.5f;

    float timeSinceLastFire = 0;

    public override void Init()
    {
        bulletSpawnPoint.localRotation = Quaternion.identity;
    }

    public override void Update()
    {
        if (timeSinceLastFire >= fireCoolDown)
        {
            timeSinceLastFire = 0;

            if (!trackTarget) Fire();
            else
            {
                //Physics cast to find enemy in range
                //if we find one
                //  point bulletSpawn at enemy
                //  Fire
                RaycastHit2D[] hits = Physics2D.BoxCastAll(turret.transform.position + turret.transform.forward * BoxCheckRadius,
                                                           new Vector2(BoxCheckRadius * 2, BoxCheckRadius * 2),
                                                           turret.transform.eulerAngles.z + 45f,
                                                           turret.transform.up, 0.0001f,
                                                           enemyMask);

                if (hits.Length > 0)
                {
                    Transform closest = hits[0].transform;
                    float dist = Vector2.Distance(closest.position, turret.transform.position);

                    for (int i = 1; i < hits.Length; i++)
                    {
                        Transform check = hits[0].transform;
                        float newDist = Vector2.Distance(hits[0].transform.position, turret.transform.position);
                        if (newDist < dist)
                        {
                            closest = check;
                            dist = newDist;
                        }
                    }

                    bulletSpawnPoint.up = closest.position - bulletSpawnPoint.position;

                    Fire();
                }
            }
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
            TurretSoundManager.Instance.PlaySound("TurretFire");
        }
        else if (bulletType == BulletType.Rocket)
        {
            Rocket rocket = TurretManager.Instance.rocketPooler.Pop() as Rocket;
            rocket.target = GameObject.Find("Damage Test")?.transform;
            bullet = rocket;
            bullet.transform.parent = null;
            TurretSoundManager.Instance.PlaySound("RocketFire");
        }
        else
        {
            bullet = TurretManager.Instance.laserPooler.Pop();
            bullet.transform.parent = turret.transform;
            TurretSoundManager.Instance.PlaySound("LaserFire");
        }

        bullet.damage = bulletDamage * (1 + turret.GetBoostMultiplier());
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;
    }

    public override void Shutdown()
    {
        if (bulletSpawnPoint != null)
            bulletSpawnPoint.localRotation = Quaternion.identity;
    }
}

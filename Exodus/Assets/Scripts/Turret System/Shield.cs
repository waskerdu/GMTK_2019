using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, ITurretMessages
{
    public float damageSoak = 150f;
    public float damageToEnemy = 50f;

    float damageTaken;
    [HideInInspector] public TurretShieldBehaviour behaviour;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Debug.Log("Shield: Collision detected!");
            collision.SendMessageUpwards("DamageEnemy", damageToEnemy);
            DamagePlanet(1);
        }
    }

    public void DamagePlanet(float damage)
    {
        damageTaken += damage;

        if (damageTaken >= damageSoak)
        {
            behaviour.DestroyTurret();
        }
    }
}

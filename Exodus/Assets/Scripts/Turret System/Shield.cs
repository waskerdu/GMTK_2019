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
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            //Debug.Log("Shield: Collision detected!");
            enemy.SendMessage("DamageEnemy", damageToEnemy);
            
        }
    }

    public void DamagePlanet(float damage)
    {
        Debug.Log(damage);
        damageTaken += damage;

        if (damageTaken >= damageSoak)
        {
            behaviour.DestroyTurret();
        }
    }
}

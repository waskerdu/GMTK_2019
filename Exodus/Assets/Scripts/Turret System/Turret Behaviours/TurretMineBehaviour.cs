using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Mine Behaviour", menuName = "Turret/Mine Behaviour")]
public class TurretMineBehaviour : TurretBehaviour
{
    public float resourcesPerSecond = 1f;
    public float planetDamagePerSecond = 1f;

    Player player;

    private void OnEnable()
    {
        player = FindObjectOfType<Player>();
    }

    public override void Update()
    {
        if (player == null) return;

        float damage = planetDamagePerSecond * Time.deltaTime;
        if (damage > 0)
            player.SendMessage("Drill", damage);

        float resources = resourcesPerSecond * Time.deltaTime;
        if (resources > 0)
            player.SendMessage("AddResources", resources);
    }
}

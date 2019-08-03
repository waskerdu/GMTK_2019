using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Mine Behaviour", menuName = "Turret/Mine Behaviour")]
public class TurretMineBehaviour : TurretBehaviour
{
    public float resourcesPerSecond = 1f;
    public float planetDamagePerSecond = 1f;
    public string soundName = "TurretDrill";

    Player player;

    private void OnEnable()
    {
        player = FindObjectOfType<Player>();
    }

    public override void Init()
    {
        TurretSoundManager.Instance.PlaySound(soundName);
    }

    public override void Update()
    {
        if (player == null) return;

        float damage = planetDamagePerSecond * Time.deltaTime * (1 + turret.GetBoostMultiplier());
        if (damage > 0)
            player.SendMessage("DamagePlanet", damage);

        float resources = resourcesPerSecond * Time.deltaTime * (1 + turret.GetBoostMultiplier());
        if (resources > 0)
            player.SendMessage("AddResources", resources);
    }
}

using Malee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Turret : MonoBehaviour, ITurretMessages
{
    public enum TurretType
    {
        Basic,
        Solar,
        Oil,
        Rocket,
        Shield,
        Laser,
        Boost
    }

    public TurretType turretType;
    [Reorderable]
    public TurretTypeLinks typeLinks;
    public SpriteRenderer boostSprite;
    public Transform bulletSpawnPoint;
    public float multiplierPerBoost = 0.5f;
    public int boosts;

    TurretBehaviour turretBehaviour;
    [HideInInspector] public TurretPosition myPosition;

    private void Awake()
    {
        turretBehaviour = Instantiate(typeLinks[0].behaviour);
        turretBehaviour.turret = this;
        boostSprite.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (turretBehaviour != null)
            turretBehaviour.Update();
    }

    public void SetTurretType(TurretType newType)
    {
        turretType = newType;
        foreach (TurretTypeLink link in typeLinks)
        {
            link.sprite.gameObject.SetActive(link.type == turretType);
            if (link.type == turretType)
            {
                if (link.behaviour == null)
                    turretBehaviour = null;
                else
                {
                    turretBehaviour.Shutdown();
                    turretBehaviour = Instantiate(link.behaviour);
                    turretBehaviour.turret = this;
                    turretBehaviour.bulletSpawnPoint = bulletSpawnPoint;
                    turretBehaviour.Init();
                }
            }
        }

        if (turretType == TurretType.Boost)
        {
            boostSprite.gameObject.SetActive(true);
            if (turretBehaviour != null)
            {
                turretBehaviour.Shutdown();
                turretBehaviour = null;
            }
        }
    }

    public float GetBoostMultiplier()
    {
        return multiplierPerBoost * boosts;
    }

    public void DamagePlanet(float damage)
    {
        Debug.Log(string.Format("{0} damage dealt to {1}.", damage, name));
    }

    public void DestroyTurret()
    {
        TurretManager.Instance.RemoveTurret(myPosition);
    }
}

[System.Serializable]
public class TurretTypeLinks : ReorderableArray<TurretTypeLink> { }

[System.Serializable]
public class TurretTypeLink
{
    public Turret.TurretType type;
    public SpriteRenderer sprite;
    public TurretBehaviour behaviour;
}

public interface ITurretMessages : IEventSystemHandler
{
    void DamagePlanet(float damage);
}

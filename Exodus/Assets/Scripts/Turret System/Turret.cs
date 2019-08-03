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
    public bool boosted;

    private void Awake()
    {
        SetBoost(false);
    }

    public void SetTurretType(TurretType newType)
    {
        turretType = newType;
        foreach (TurretTypeLink link in typeLinks)
            link.sprite.gameObject.SetActive(link.type == turretType);
    }

    public void SetBoost(bool toBoost = true)
    {
        boosted = toBoost;
        boostSprite.gameObject.SetActive(toBoost);
    }

    public void DamagePlanet(float damage)
    {
        Debug.Log(string.Format("{0} damage dealt to {1}.", damage, name));
    }
}

[System.Serializable]
public class TurretTypeLinks : ReorderableArray<TurretTypeLink> { }

[System.Serializable]
public class TurretTypeLink
{
    public Turret.TurretType type;
    public SpriteRenderer sprite;
}

public interface ITurretMessages : IEventSystemHandler
{
    void DamagePlanet(float damage);
}

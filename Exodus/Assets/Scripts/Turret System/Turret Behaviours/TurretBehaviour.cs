using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : ScriptableObject
{
    [HideInInspector] public Turret turret;
    [HideInInspector] public Transform bulletSpawnPoint;

    public virtual void Update() { }
    public virtual void Init() { }
    public virtual void Shutdown() { }
}

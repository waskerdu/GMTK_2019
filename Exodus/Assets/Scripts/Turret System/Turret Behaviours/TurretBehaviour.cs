using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : ScriptableObject
{
    [HideInInspector] public Turret turret;

    public virtual void Update() { }
}

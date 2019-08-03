using Malee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretManager : MonoBehaviour, ITurretManagerMessages
{
    static TurretManager instance;
    public static TurretManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<TurretManager>();
            return instance;
        }
    }

    public enum Biome
    {
        Forest,
        Desert,
        Ocean,
        Mountain
    }

    public GameObject planet;
    public Transform turretDropPosition;
    public LayerMask planetLayer;
    public GameObject ghostTurret;
    public GameObject turretPositionPrefab;
    public Turret turretPrefab;
    public float turretHeightOffset = 0.5f;
    public int spokes = 20;

    GhostTurret ghostTurretInstance;
    TurretPositions turretPositions;
    List<Biome> biomeList;

    private void Awake()
    {
        float theta = 360f / spokes;
        turretPositions = new TurretPositions();

        for (int i = 0; i < spokes; i++)
        {
            TurretPosition pos = new TurretPosition();
            pos.turrets = new List<Turret>();
            pos.positionObject = Instantiate(turretPositionPrefab, planet.transform);
            pos.positionObject.name = string.Format("Turret Position {0}", i);
            pos.positionObject.transform.localPosition = Vector3.zero;
            pos.positionObject.transform.Rotate(Vector3.forward * theta * -i);
            pos.positionObject.transform.Translate(Vector3.up * turretDropPosition.position.y);
            if (i > 0)
            {
                turretPositions[turretPositions.Count - 1].rightPosition = pos;
                pos.leftPosition = turretPositions[turretPositions.Count - 1];
            }
            turretPositions.Add(pos);
        }

        turretPositions[0].leftPosition = turretPositions[turretPositions.Count - 1];
        turretPositions[turretPositions.Count - 1].rightPosition = turretPositions[0];

        ghostTurretInstance = Instantiate(ghostTurret, transform).GetComponent<GhostTurret>();
        ghostTurretInstance.name = "Ghost Turret";
        ghostTurretInstance.gameObject.SetActive(false);

        ghostTurretInstance.targetPosition = turretPositions[0];
        ghostTurretInstance.topPosition = turretDropPosition;
        ghostTurretInstance.turretHeightOffset = turretHeightOffset;

        List<int> biomeTesting = new List<int>();

        for (int i = 0; i < spokes; i++)
        {
            biomeTesting.Add(i % 4);
        }

        SetBiomeData(biomeTesting.ToArray());
    }

    private void Start()
    {
        //Set biome data after we receive the biomes
        for (int i = 0; i < turretPositions.Count; i++)
            turretPositions[i].biome = biomeList[i];
    }

    public void GameWon()
    {
        Debug.Log("Turret Manager: Game Won!");
    }

    public void SetDifficulty(int difficulty)
    {
        Debug.Log(string.Format("Turret Manager: Difficulty set to {0}.", difficulty));
    }

    public void ShowGhostTurret(bool toShow = true)
    {
        ghostTurretInstance.gameObject.SetActive(toShow);
        ghostTurretInstance.transform.position = turretDropPosition.position;
    }

    public void PlaceTurret()
    {
        TurretPosition pos = GetTopPosition();

        Turret newTurret = Instantiate(turretPrefab, pos.positionObject.transform);
        newTurret.transform.localRotation = Quaternion.identity;
        newTurret.transform.localPosition = new Vector3(0, turretHeightOffset * pos.turrets.Count, 0);
        newTurret.gameObject.name = "New Turret";
        newTurret.SetTurretType(Turret.TurretType.Basic);

        pos.turrets.Add(newTurret);
        ghostTurretInstance.gameObject.SetActive(false);

        List<TurretPosition> positions = pos.GetLeftmostPositionWithTurret().GetTurrets();
        foreach (TurretPosition position in positions)
            position.SetTurretType(positions.Count);
    }

    public TurretPosition GetTopPosition()
    {
        float rot = planet.transform.eulerAngles.z;
        int index = Mathf.RoundToInt(Utilities.Map(rot >= 0 ? rot : 360f + rot, 0, 360f, 0, spokes)) % spokes;
        return turretPositions[index];
    }

    public void RemoveTurret()
    {
        TurretPosition pos = GetTopPosition();

        foreach (Turret turret in pos.turrets)
            Destroy(turret.gameObject);

        pos.turrets = new List<Turret>();

        //TODO: Set turret types
        if (pos.leftPosition.turrets.Count > 0)
        {
            List<TurretPosition> positions = pos.leftPosition.GetLeftmostPositionWithTurret().GetTurrets();
            foreach (TurretPosition position in positions)
                position.SetTurretType(positions.Count);
        }

        if (pos.rightPosition.turrets.Count > 0)
        {
            List<TurretPosition> positions = pos.rightPosition.GetTurrets();
            foreach (TurretPosition position in positions)
                position.SetTurretType(positions.Count);
        }
    }

    public void SetBiomeData(int[] biomes)
    {
        biomeList = new List<Biome>();
        foreach (int biome in biomes)
            biomeList.Add((Biome)biome);
    }
}

[System.Serializable]
public class TurretPositions : ReorderableArray<TurretPosition> { }

[System.Serializable]
public class TurretPosition
{
    public GameObject positionObject;
    public List<Turret> turrets;
    public TurretManager.Biome biome;
    public TurretPosition leftPosition;
    public TurretPosition rightPosition;

    public void SetTurretType(int chainQuantity)
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            if (i < turrets.Count - 1)
            {
                turrets[i].SetTurretType(Turret.TurretType.Boost);
                if (turrets.Count > 1)
                    turrets[i].SetBoost(true);
            }
            else
            {
                if (chainQuantity == 1)
                    switch (biome)
                    {
                        case TurretManager.Biome.Forest:
                            turrets[i].SetTurretType(Turret.TurretType.Basic);
                            break;

                        case TurretManager.Biome.Desert:
                            turrets[i].SetTurretType(Turret.TurretType.Solar);
                            break;

                        case TurretManager.Biome.Ocean:
                            turrets[i].SetTurretType(Turret.TurretType.Oil);
                            break;

                        case TurretManager.Biome.Mountain:
                            turrets[i].SetTurretType(Turret.TurretType.Rocket);
                            break;
                    }
                else if (chainQuantity == 2)
                    turrets[i].SetTurretType(Turret.TurretType.Shield);
                else
                    turrets[i].SetTurretType(Turret.TurretType.Laser);
            }
        }
    }

    public TurretPosition GetLeftmostPositionWithTurret()
    {
        if (leftPosition.turrets.Count > 0)
            return leftPosition.GetLeftmostPositionWithTurret();
        else
            return this;
    }

    public List<TurretPosition> GetTurrets()
    {
        if (rightPosition.turrets.Count > 0)
        {
            List<TurretPosition> positions = rightPosition.GetTurrets();
            positions.Add(this);
            return positions;
        }
        else
        {
            List<TurretPosition> positions = new List<TurretPosition>();
            positions.Add(this);
            return positions;
        }
    }
}

public interface ITurretManagerMessages : IEventSystemHandler
{
    void ShowGhostTurret(bool toShow);
    void PlaceTurret();
    void RemoveTurret();
    void SetDifficulty(int difficulty);
    void GameWon();
    void SetBiomeData(int[] biomes);
}

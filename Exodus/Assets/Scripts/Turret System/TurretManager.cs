using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretManager : MonoBehaviour, ITurretMessages
{
    public GameObject planet;
    public Transform turretDropPosition;
    public LayerMask planetLayer;
    public GameObject ghostTurret;
    public Turret turretPrefab;
    public int spokes = 20;

    GameObject ghostTurretInstance;
    List<GameObject> turretPositions;

    private void Awake()
    {
        float theta = 360f / spokes;
        turretPositions = new List<GameObject>();

        for (int i = 0; i < spokes; i++)
        {
            turretPositions.Add(new GameObject(string.Format("Turret Position {0}", i)));
            turretPositions[i].transform.parent = planet.transform;
            turretPositions[i].transform.localPosition = Vector3.zero;
            turretPositions[i].transform.Rotate(Vector3.forward * theta * i);
            turretPositions[i].transform.Translate(Vector3.up * turretDropPosition.position.y);
        }

        ghostTurretInstance = Instantiate(ghostTurret, transform);
        ghostTurretInstance.GetComponent<GhostTurret>().planet = planet.transform;
        ghostTurretInstance.name = "Ghost Turret";
        ghostTurretInstance.SetActive(false);

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
        ghostTurretInstance.SetActive(toShow);
        ghostTurretInstance.transform.position = turretDropPosition.position;
    }

    public void PlaceTurret()
    {
        ghostTurretInstance.SetActive(false);
        Turret newTurret = Instantiate(turretPrefab, planet.transform);
        newTurret.transform.position = ghostTurretInstance.transform.position;
        newTurret.transform.localEulerAngles = -planet.transform.eulerAngles;// + new Vector3(0, 0, 90f);
        newTurret.gameObject.name = "New Turret";
        newTurret.SetTurretType(Turret.TurretType.Basic);
    }

    public void RemoveTurret()
    {
        //
    }
}

public interface ITurretMessages : IEventSystemHandler
{
    void ShowGhostTurret(bool toShow);
    void PlaceTurret();
    void RemoveTurret();
    void SetDifficulty(int difficulty);
    void GameWon();
}

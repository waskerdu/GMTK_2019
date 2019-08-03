using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour, IPlayerMessages
{
    public float spinSpeed = 100.0f;
    public float jumpPower = 10.0f;
    public GameObject turretSystem;
    public GameObject sliderObj;
    public GameObject camObj;
    public GameObject materialUi;
    public GameObject integrityUi;
    Slider slider;
    public float minZoom = 1.0f;
    public float maxZoom = 5.0f;
    public float zoomSpeed = 1.0f;
    public float zoom = 0.0f;
    public float material = 3.0f;
    public float integrity = 1.0f;
    public float damageMultiplier = 0.01f;
    public int numForests = 7;
    public int numDeserts = 5;
    public int numOceans = 5;
    
    public int numMountains = 3;
    List<int> biomes = new List<int>();
    Rigidbody2D planetRb;
    Rigidbody2D playerRb;
    Camera cam;
    float timer = 0.0f;
    public float confirmTime = 1.0f;
    bool place = true;
    bool ready = true;
    void Start()
    {
        playerRb = transform.GetChild(0).GetComponent<Rigidbody2D>();
        planetRb = transform.GetChild(1).GetComponent<Rigidbody2D>();
        cam = camObj.GetComponent<Camera>();
        slider = sliderObj.GetComponent<Slider>();
        int i;
        List<int> tempBiomes = new List<int>();
        for (i = 0; i < numForests; i++)
        {
            tempBiomes.Add(0);
        }
        for (i = 0; i < numDeserts; i++)
        {
            tempBiomes.Add(1);
        }
        for (i = 0; i < numOceans; i++)
        {
            tempBiomes.Add(2);
        }
        for (i = 0; i < numMountains; i++)
        {
            tempBiomes.Add(3);
        }
        while (tempBiomes.Count>0)
        {
            i = Random.Range(0,tempBiomes.Count-1);
            biomes.Add(tempBiomes[i]);
            tempBiomes.RemoveAt(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        planetRb.angularVelocity = Input.GetAxisRaw("Horizontal")*spinSpeed*Time.deltaTime;
        if(Input.GetButtonDown("Jump")){playerRb.velocity=Vector2.up*jumpPower;}
        if(Input.GetButtonDown("Fire1"))
        {
            place = true;
            timer = confirmTime;
            turretSystem.SendMessage("ShowGhostTurret",true);
        }
        if(Input.GetButtonDown("Fire2"))
        {
            place = false;
            timer = confirmTime;
            turretSystem.SendMessage("ShowGhostTurret",true);
        }
        if(Input.GetButton("Fire1") || Input.GetButton("Fire2"))
        {
            if(timer>0)
            {
                timer-=Time.deltaTime;
            }
            else
            {
                if (ready)
                {
                    if(place)
                    {
                        if (material>=1.0f)
                        {
                            turretSystem.SendMessage("PlaceTurret");
                            material-=1.0f;
                        }
                    }
                    else{turretSystem.SendMessage("RemoveTurret");}
                    turretSystem.SendMessage("ShowGhostTurret",false);
                    timer = 0.0f;
                    ready = false;
                }
                
            }
        }
        if(Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2"))
        {
            turretSystem.SendMessage("ShowGhostTurret",false);
            timer = 0.0f;
            ready = true;
        }
        if(timer == 0.0f)
        {
            sliderObj.SetActive(false);
        }
        else
        {
            sliderObj.SetActive(true);
            slider.value = 1.0f - timer/confirmTime;
        }
        zoom-=Input.GetAxisRaw("Vertical") * zoomSpeed * Time.deltaTime;
        zoom = Mathf.Clamp(zoom,0,1);
        cam.orthographicSize = (maxZoom-minZoom)*EaseInOutQuad(zoom)+minZoom;
    }

    float EaseOutQuad(float start, float end, float value)
    {
        end -= start;
        return -end * value * (value - 2) + start;
    }

    float EaseInOutQuad (float t) { return t<.5 ? 2*t*t : -1+(4-2*t)*t; }

    public void DamagePlanet(float damage)
    {
        //Debug.Log(string.Format("Player: {0} drill damage!", damage));
        //TODO: actually damage planet
        integrity-=damage*damageMultiplier;
        integrityUi.GetComponent<TextMeshProUGUI>().SetText((Mathf.Round(integrity*1000)/10).ToString()+"%");
    }

    public void AddResources(float resources)
    {
        //Debug.Log(string.Format("Player: {0} resources added!", resources));
        //TODO: actually add resources
        material+=resources;
        materialUi.GetComponent<TextMeshProUGUI>().SetText("Material: "+(Mathf.Round(material*10)/10).ToString());
    }
}

public interface IPlayerMessages : IEventSystemHandler
{
    void DamagePlanet(float damage);
    void AddResources(float resources);
}
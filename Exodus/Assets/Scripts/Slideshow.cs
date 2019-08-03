using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slideshow : MonoBehaviour
{
    public float timer = 0.0f;
    public float slideTime = 1;
    public int slide = 0;    
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(slide < transform.childCount+1)
        {
            
            if (timer>0.0f){timer-=Time.deltaTime;}
            else
            {
                timer = slideTime;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                if(slide < transform.childCount)transform.GetChild(slide).gameObject.SetActive(true);
                slide++;
            }
        }
        else
        {
            SendMessageUpwards("SlideshowFinished");
        }
    }
}

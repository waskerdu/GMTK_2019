using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWave : MonoBehaviour
{
    [SerializeField] float expandSpeed = 0.1f;
    [SerializeField] float expandTime = 0.8f;
    [SerializeField] float opacityDrainSpeed = 0.1f;
    float timer;
    Vector3 originalScaling;
    Color color;
    SpriteRenderer spriteRenderer;
    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScaling = transform.localScale;
        color = spriteRenderer.color;
        timer = expandTime;

    }
    private void Update()
    {
        transform.localScale += new Vector3(expandSpeed,expandSpeed,expandSpeed) * Time.deltaTime;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - (Time.deltaTime * opacityDrainSpeed));
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            transform.localScale = originalScaling;
            spriteRenderer.color = color;
            gameObject.SetActive(false);
        }
    }
}

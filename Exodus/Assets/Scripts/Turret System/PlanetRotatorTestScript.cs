using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotatorTestScript : MonoBehaviour
{
    public float rotationSpeed = 3f;

    private void Update()
    {
        float rotation = Input.GetAxisRaw("Horizontal");
        if (rotation != 0)
            transform.Rotate(new Vector3(0, 0, rotation * Time.deltaTime * rotationSpeed));
    }
}

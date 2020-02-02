using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool isHit = false;
    private Renderer wallRenderer; 
    private Vector3 hitPosition;

    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
        hitPosition = transform.position + Vector3.forward * 0.01f;
    }

    void Update()
    {
        if (isHit)
        {
            wallRenderer.material.SetColor("_Color", Color.red);
            transform.position = hitPosition;
        }
        else
        {
            wallRenderer.material.SetColor("_Color", Color.black);

        }
    }

     
}

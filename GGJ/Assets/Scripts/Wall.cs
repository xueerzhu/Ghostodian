using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool isHit = false;

    public GameObject brokenFenceModel;
    public GameObject fixedFenceModel;

    private Renderer wallRenderer; 
    private Vector3 hitPosition;

    private bool wallChangeAudioPlayed = false;

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
            
            if (!wallChangeAudioPlayed)
            {
                wallChangeAudioPlayed = true;
                GetComponent<AudioSource>().Play();

                brokenFenceModel.active = false;
                fixedFenceModel.active = true;

            }
            


            // if (transform.localScale.x < 1.101f){
            //     transform.localScale += 0.002f*Vector3.right; 
            // }
            // if (transform.localScale.y < 0.101f){
            //     transform.localScale += 0.002f*Vector3.up; 
            // }
            if (transform.localScale.z < 1.0f){
                transform.localScale += 0.02f*Vector3.forward; 
            }
        }
        else
        {
            wallRenderer.material.SetColor("_Color", Color.black);

        }
    }

     
}
